using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels.User;
using System.Linq.Expressions;
using static Models.Entities.Concrete.Shop;

namespace Business.Services.Concrete
{
    public class SellerApplicationService : ISellerApplicationService
    {
        private readonly ISellerApplicationRepository _sellerApplicationRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IShopService _shopService;
        private readonly IConfiguration _configuration;

        public SellerApplicationService(
            ISellerApplicationRepository sellerApplicationRepository,
            UserManager<AppUser> userManager,
            IMapper mapper,
            IEmailService emailService,
            IShopService shopService,
            IConfiguration configuration)
        {
            _sellerApplicationRepository = sellerApplicationRepository;
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
            _shopService = shopService;
            _configuration = configuration;
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
        public async Task<IDataResult<IEnumerable<SellerApplication>>> GetApplicationsByStatusAsync(ApplicationStatus status)
        {
            var applications = await _sellerApplicationRepository.GetApplicationsByStatusAsync(status);

            return applications is not null && applications.Any()
                ? new SuccessDataResult<IEnumerable<SellerApplication>>(applications)
                : new ErrorDataResult<IEnumerable<SellerApplication>>(Messages.ApplicationNotFound);
        }


        #endregion

        #region Create

        public async Task<IResult> CreateSellerApplicationAsync(SellerRegistrationViewModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return existingUser.IsSeller
                    ? new ErrorResult("You are already a seller. You cannot apply again.")
                    : new ErrorResult("This email is already registered in the system. Please use another email address for your seller application.");
            }

            var existingApplication = await _sellerApplicationRepository.GetAllAsync(a => a.Email == model.Email && a.Status == ApplicationStatus.Pending);
            if (existingApplication != null && existingApplication.Any())
            {
                return new ErrorResult(Messages.ApplicationAlreadyExist);
            }

            var sellerApplication = _mapper.Map<SellerApplication>(model);
            sellerApplication.Status = ApplicationStatus.Pending;

            var result = await _sellerApplicationRepository.CreateAsync(sellerApplication);

            return result > 0
               ? new SuccessResult(Messages.CreateSellerApplicationSuccess)
               : new ErrorResult(Messages.CreateSellerApplicationError);
        }

        #endregion

        #region Approve

        public async Task<IResult> ApproveSellerAsync(Guid id)
        {
            
            var application = await _sellerApplicationRepository.GetByIdAsync(id);
            if (application == null)
            {
                return new ErrorResult(Messages.ApplicationNotFound);
            }
            if (application.Status == ApplicationStatus.Approved)
            {
                return new ErrorResult(Messages.ApplicationAlreadyApproved);
            }
            if (application.Status == ApplicationStatus.Rejected)
            {
                return new ErrorResult(Messages.ApplicationAlreadyRejected);
            }

            //using var transaction = await _sellerApplicationRepository.BeginTransactionAsync();

            try
            {
                //Create user(seller)
                var user = await _userManager.FindByEmailAsync(application.Email);
                if (user == null)
                    user = new AppUser
                    {
                        UserName = application.Email,
                        Email = application.Email,
                        Name = application.Name,
                        Address = application.Address,
                        PhoneNumber = application.ContactNumber,
                        IsSeller = true,
                        IsActive = true,
                        EmailConfirmed = true
                    };
                //Random şifre oluşturacaksın
                var defaultSellerPassword = _configuration["SellerSettings:DefaultSellerPassword"];
                if (string.IsNullOrEmpty(defaultSellerPassword))
                {
                    return new ErrorResult(Messages.PasswordError);
                }
                var createUserResult = await _userManager.CreateAsync(user, defaultSellerPassword);

                if (!createUserResult.Succeeded)
                {
                    return new ErrorResult(Messages.CreateUserError);
                }

                //Add to role 
                var roleResult = await _userManager.AddToRoleAsync(user, "Seller");
                if (!roleResult.Succeeded)
                {
                    return new ErrorResult(Messages.UserRoleError);
                }
                application.UserId = user.Id;
                application.Status = ApplicationStatus.Approved;

                var updateResult = await _sellerApplicationRepository.UpdateAsync(application);
                if (updateResult <= 0)
                {
                    return new ErrorResult(Messages.UpdateSellerApplicationError);
                }


                //Create seller's Shop
                var shop = new Shop
                {
                    Name = application.StoreName,
                    SellerId = user.Id,
                    BusinessPhoneNumber = application.ContactBusinessNumber,
                    Address = application.Address,
                    TaxNumber = application.TaxNumber,
                    IsActive = true,
                    Status = ShopStatus.Approved
                };
                var createShopResult = await _shopService.CreateShopAsync(shop);
                if (!createShopResult.Success)
                { 
                    return new ErrorResult(Messages.CreateShopError);
                }

                //Send Email to seller
                var emailResult = await _emailService.SendSellerApprovedEmail(application.Email, application.Name);
                if (!emailResult)
                {
                    return new ErrorResult(Messages.ErrorSentEmail);
                }

                return new SuccessResult(Messages.ApprovedApplicationSuccess);
            }
            catch (Exception ex)
            {
                //await transaction.RollbackAsync();

                Console.WriteLine(ex.Message);
                return new ErrorResult(Messages.ApprovedApplicationError);

            }
        }
        #endregion

        #region Reject
        public async Task<IResult> RejectSellerAsync(Guid id)
        {
            var application = await _sellerApplicationRepository.GetByIdAsync(id);
            if (application == null)
            {
                return new ErrorResult(Messages.ApplicationNotFound);
            }
            if (application.Status == ApplicationStatus.Approved)
            {
                return new ErrorResult(Messages.ApplicationAlreadyApproved);
            }
            if (application.Status == ApplicationStatus.Rejected)
            {
                return new ErrorResult(Messages.ApplicationAlreadyRejected);
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






    }
}
