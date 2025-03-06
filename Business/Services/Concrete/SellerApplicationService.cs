using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels.User;
using System.Linq.Expressions;

namespace Business.Services.Concrete
{
    public class SellerApplicationService:ISellerApplicationService
    {
        private readonly ISellerApplicationRepository _sellerApplicationRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IShopService _shopService;

        public SellerApplicationService(
            ISellerApplicationRepository sellerApplicationRepository, 
            UserManager<AppUser> userManager,
            IMapper mapper,
             IEmailService emailService,
             IShopService shopService)
        {
            _sellerApplicationRepository = sellerApplicationRepository;
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
            _shopService = shopService;
        }

        #region Read
        public async Task<IDataResult<SellerApplication>> GetSellerApplicationByIdAsync(Guid id)
        {
            var sellerApplication = await _sellerApplicationRepository.GetByIdAsync(id);
            return sellerApplication is not null
                ? new SuccessDataResult<SellerApplication>(sellerApplication)
                : new ErrorDataResult<SellerApplication>(Messages.ApplicationNotFound);
        }
        public async Task<IDataResult<IEnumerable<SellerApplication>>> GetAllSellerApplicationsAsync(Expression<Func<SellerApplication, bool>> predicate)
        {
            var applicationList = await _sellerApplicationRepository.GetAllAsync(predicate);
            return applicationList is not null && applicationList.Any()
                ? new SuccessDataResult<IEnumerable<SellerApplication>>(applicationList)
                : new ErrorDataResult<IEnumerable<SellerApplication>>(Messages.EmptySellerApplicationList);
        }



        #endregion

        #region Act

        public async Task<IResult> ApproveSellerAsync(Guid id)
        {
            var application = await _sellerApplicationRepository.GetByIdAsync(id);
            if(application == null)
            {
                return new ErrorResult(Messages.ApplicationNotFound);
            }
            if (application.Status == ApplicationStatus.Approved)
            {
                return new ErrorResult(Messages.ExistingApprovedSellerApplicationError);
            }
            if (application.Status == ApplicationStatus.Rejected)
            {
                return new ErrorResult("This application was previously rejected and cannot be approved.");
            }
            using var transaction = await _sellerApplicationRepository.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(application.Email);
            
                    user = new AppUser
                    {
                        UserName = application.Email,
                        Email = application.Email,
                        Name = application.Name,
                        Address = application.Address,
                        PhoneNumber = application.ContactNumber,
                        IsSeller = true
                    };

                    var createUserResult = await _userManager.CreateAsync(user, "Seller.1");
                    if (!createUserResult.Succeeded)
                    {
                        return new ErrorResult(Messages.CreateUserError);
                    }
                    var roleResult = await _userManager.AddToRoleAsync(user, "Seller");
                    if (!roleResult.Succeeded)
                    {
                        return new ErrorResult(Messages.UserRoleError);
                    }
                    application.UserId = user.Id;
                    application.Status = ApplicationStatus.Approved;
                    var shop = new Shop
                    {
                        Name = application.StoreName,
                        SellerId = user.Id,
                        ContactNumber = application.ContactNumber,
                        Address = application.Address,
                        TaxNumber = application.TaxNumber,
                        IsActive = true,
                        Status = "Approved"
                    };
                    await _shopService.CreateShopAsync(shop);
                    

                    application.ShopId = shop.Id;

                     var updateResult = await _sellerApplicationRepository.UpdateAsync(application);
                    if (updateResult <= 0)
                    {
                        return new ErrorResult(Messages.UpdateSellerApplicationError);
                    }


                    var emailSubject = "Your Seller Application has been Approved!";
                    var emailBody = $"Hello {application.Name},\n\nCongratulations! " +
                        $"Your vendor application has been approved. " +
                        $"Now you can create your store and start selling your products. " +
                        $"To log in to the site, use your e-mail address and the password below." +
                        $"For your security, do not forget to change your password in the 'Change Password' field." +
                        $"Password= Seller.1";

                    var emailResult = await _emailService.SendEmailAsync(application.Email, emailSubject, emailBody);

                    if (!emailResult)
                    {
                        return new ErrorResult(Messages.ErrorSentEmail);
                }
                await transaction.CommitAsync();

                return new SuccessResult(Messages.ApprovedApplicationSuccess);
        }
        catch
        {
            await transaction.RollbackAsync();
            return new ErrorResult("An error occurred while processing the approval.");
        }
        }

        public async Task<IResult> RejectSellerAsync(Guid id)
        {
            var application = await _sellerApplicationRepository.GetByIdAsync(id);
            if (application == null)
            {
                return new ErrorResult(Messages.ApplicationNotFound);
            }
            if (application.Status == ApplicationStatus.Approved)
            {
                return new ErrorResult(Messages.ExistingApprovedSellerApplicationError);
            }
            if (application.Status == ApplicationStatus.Rejected)
            {
                return new ErrorResult(Messages.ExistingRejectedSellerApplicationError);
            }

            application.Status = ApplicationStatus.Rejected;
            var updateResult = await _sellerApplicationRepository.UpdateAsync(application);
            if (updateResult <= 0)
            {
                return new ErrorResult(Messages.UpdateSellerApplicationError);
            }
            var emailSubject = "Your Seller Application has been Rejected!";
            var emailBody = $"Hello {application.Name}, " +
                $"Your seller application to EzyShop has been reviewed. " +
                $"Unfortunately, we reject your request due to incompatibilities in your application." +
                $"You can contact customer service to get more information.";

            var emailResult = await _emailService.SendEmailAsync(application.Email, emailSubject, emailBody);

            
            return new SuccessResult(Messages.RejectedApplicationSuccess);
        }

        #endregion
        #region Create

        public async Task<IResult> CreateSellerApplicationAsync(SellerRegistrationViewModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                if (existingUser.IsSeller)
                {
                    return new ErrorResult("You are already a seller. You cannot apply again.");
                }
                return new ErrorResult("This email is already registered in the system. Please use another email address for your seller application.");
            }

            var existingApplication = await _sellerApplicationRepository.GetAllAsync(a => a.Email == model.Email && a.Status == ApplicationStatus.Pending);
            if (existingApplication != null && existingApplication.Any())
            {
                return new ErrorResult(Messages.ExistingSellerApplicationError);
            }

            var sellerApplication = _mapper.Map<SellerApplication>(model);
            sellerApplication.Status = ApplicationStatus.Pending;

            var result = await _sellerApplicationRepository.CreateAsync(sellerApplication);

            return result > 0
               ? new SuccessResult(Messages.CreateSellerApplicationSuccess)
               : new ErrorResult(Messages.CreateSellerApplicationError);
        }

        #endregion


       


    }
}
