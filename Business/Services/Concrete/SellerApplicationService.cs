using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Pagination;
using Core.Security;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels.SellerApplication;

namespace Business.Services.Concrete;

public class SellerApplicationService : BaseService, ISellerApplicationService
{
    private readonly ISellerApplicationRepository _sellerApplicationRepo;
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IShopService _shopService;
    private readonly ISellerRepository _sellerRepo;

    public SellerApplicationService(
        ISellerApplicationRepository sellerApplicationRepo,
        UserManager<AppUser> userManager,
        IEmailService emailService,
        IShopService shopService,
        ISellerRepository sellerRepo,
        IMapper mapper,
        IConfiguration config,
        ICurrentUserService currentUser)
    : base(mapper, config, currentUser)
    {
        _sellerApplicationRepo = sellerApplicationRepo;
        _userManager = userManager;
        _shopService = shopService;
        _sellerRepo = sellerRepo;
        _emailService = emailService;
    }

    #region Crud

    public async Task<IResult> CreateSellerApplicationAsync(SellerApplicationViewModel model)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            return new ErrorResult(Messages.EmailAlreadyRegistered);
        }

        if ((await _sellerApplicationRepo.ExistsAsync(a =>
            a.Email == model.Email &&
            a.Status == ApplicationStatus.Pending &&
            !a.IsDeleted)))
        {
            return new ErrorResult(Messages.ApplicationAlreadyExist);
        }


        var sellerApplication = Mapper.Map<SellerApplication>(model);
        sellerApplication.Status = ApplicationStatus.Pending;

        var result = await _sellerApplicationRepo.CreateAsync(sellerApplication);

        return result > 0
           ? new SuccessResult(Messages.CreateSellerApplicationSuccess)
           : new ErrorResult(Messages.CreateSellerApplicationError);
    }

    public async Task<IResult> DeleteSellerApplicationAsync(Guid sellerApplicationId)
    {
        var exists = await _sellerApplicationRepo.ExistsAsync(s => s.Id == sellerApplicationId && !s.IsDeleted);
        if (!exists)
            return new ErrorResult(Messages.ApplicationNotFound);

        var result = await _sellerApplicationRepo.SoftDeleteAsync(sellerApplicationId);

        return result > 0
        ? new SuccessResult(Messages.DeleteSellerApplicationSuccess)
        : new ErrorResult(Messages.DeleteSellerApplicationError);
    }

    public async Task<IDataResult<SellerApplication>> GetSellerApplicationByIdAsync(Guid sellerApplicationId)
    {
        var sellerApplication = await _sellerApplicationRepo.GetByIdAsync(sellerApplicationId);
        return sellerApplication is not null
            ? new SuccessDataResult<SellerApplication>(sellerApplication)
            : new ErrorDataResult<SellerApplication>(Messages.ApplicationNotFound);
    }
    #endregion

    #region Read


    public async Task<IDataResult<PaginatedList<SellerApplicationViewModel>>> GetPaginatedAllApplicationsAsync(int page, int pageSize)
    {
        var pagedApplications = await _sellerApplicationRepo
            .GetPaginatedAsync(app => !app.IsDeleted, page, pageSize);

        var viewModels = Mapper.Map<IEnumerable<SellerApplicationViewModel>>(pagedApplications.Items);

        var result = new PaginatedList<SellerApplicationViewModel>(
            viewModels,
            pagedApplications.TotalItems,
            pagedApplications.Page,
            pagedApplications.PageSize);

        return result.Items.Any()
            ? new SuccessDataResult<PaginatedList<SellerApplicationViewModel>>(result)
            : new ErrorDataResult<PaginatedList<SellerApplicationViewModel>>(Messages.ApplicationNotFound);
    }


    public async Task<IDataResult<PaginatedList<SellerApplicationViewModel>>> GetPaginatedApplicationsByStatusAsync(
    ApplicationStatus status, int page, int pageSize)
    {
        var pagedApplications = await _sellerApplicationRepo
            .GetPaginatedAsync(app => !app.IsDeleted && app.Status == status, page, pageSize);

        var viewModels = Mapper.Map<IEnumerable<SellerApplicationViewModel>>(pagedApplications.Items);

        var result = new PaginatedList<SellerApplicationViewModel>(
            viewModels,
            pagedApplications.TotalItems,
            pagedApplications.Page,
            pagedApplications.PageSize);

        return result.Items.Any()
            ? new SuccessDataResult<PaginatedList<SellerApplicationViewModel>>(result)
            : new ErrorDataResult<PaginatedList<SellerApplicationViewModel>>(Messages.ApplicationNotFound);
    }

    #endregion

    #region Approve

    public async Task<IResult> ApproveSellerAsync(Guid id)
    {
        var application = await _sellerApplicationRepo.GetByIdAsync(id);
        if (application == null)
            return new ErrorResult(Messages.ApplicationNotFound);
        if (application.Status == ApplicationStatus.Approved)
            return new ErrorResult(Messages.ApplicationAlreadyApproved);
        if (application.Status == ApplicationStatus.Rejected)
            return new ErrorResult(Messages.ApplicationAlreadyRejected);

        using var transaction = await _sellerApplicationRepo.BeginTransactionAsync();

        try
        {
            //User
            var user = await _userManager.FindByEmailAsync(application.Email);
            if (user == null)
                user = new AppUser
                {
                    UserName = application.Email,
                    Email = application.Email,
                    Name = application.Name,
                    Surname = application.Surname,
                    PhoneNumber = application.ContactBusinessNumber,
                    EmailConfirmed = true
                };

            var password = Config["SellerSettings:DefaultSellerPassword"];
            if (string.IsNullOrEmpty(password))
                return new ErrorResult(Messages.PasswordError);

            var createUserResult = await _userManager.CreateAsync(user, password);

            if (!createUserResult.Succeeded)
                return new ErrorResult(Messages.CreateUserError);


            // Role 
            var roleResult = await _userManager.AddToRoleAsync(user, "Seller");
            if (!roleResult.Succeeded)
                return new ErrorResult(Messages.UserRoleError);



            // Update Application
            application.UserId = user.Id;
            application.Status = ApplicationStatus.Approved;

            var updateApp = await _sellerApplicationRepo.UpdateAsync(application);
            if (updateApp <= 0)
                return new ErrorResult(Messages.UpdateSellerApplicationError);

            //Seller
            var seller = new Seller
            {
                UserId = user.Id,
                SellerApplicationId = application.Id
            };
            var sellerResult = await _sellerRepo.CreateAsync(seller);

            if (sellerResult <= 0)
                return new ErrorResult(Messages.CreateSellerError);

            application.SellerId = seller.Id;
            await _sellerApplicationRepo.UpdateAsync(application);



            // Shop
            var shop = new Shop
            {
                SellerId = user.Id,
                Name = application.StoreName,
                ShopAddress = application.ShopAddress,
                TaxNumber = application.TaxNumber,
                Status = Shop.ShopStatus.Approved,
            };
            var createShopResult = await _shopService.CreateShopAsync(shop);
            if (!createShopResult.Success)
                return new ErrorResult(Messages.CreateShopError);

            await transaction.CommitAsync();

            // Send Email  
            var emailResult = await _emailService.SendSellerApprovedEmail(application.Email, application.Name);
            if (!emailResult)
                return new ErrorResult(Messages.ErrorSentEmail);

            return new SuccessResult(Messages.ApprovedApplicationSuccess);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return new ErrorResult(Messages.ApprovedApplicationError);

        }
    }
    #endregion

    #region Reject
    public async Task<IResult> RejectSellerAsync(Guid id)
    {
        var application = await _sellerApplicationRepo.GetByIdAsync(id);
        if (application == null)
            return new ErrorResult(Messages.ApplicationNotFound);
        if (application.Status == ApplicationStatus.Approved)
            return new ErrorResult(Messages.ApplicationAlreadyApproved);
        if (application.Status == ApplicationStatus.Rejected)
            return new ErrorResult(Messages.ApplicationAlreadyRejected);
       
      

        application.Status = ApplicationStatus.Rejected;
        var updateResult = await _sellerApplicationRepo.UpdateAsync(application);
        if (updateResult <= 0)
          return new ErrorResult(Messages.UpdateSellerApplicationError);
        
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
