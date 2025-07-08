﻿using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Interfaces;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels.Auth;

namespace Business.Services.Concrete;

public class SellerService : BaseService, ISellerService
{
    private readonly ISellerRepository _sellerRepo;
    private readonly IShopRepository _shopRepo;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    public SellerService(
     IShopRepository shopRepo,
     ISellerRepository sellerRepo,
     ICurrentUserService currentUserService,
     UserManager<AppUser> userManager,
     RoleManager<AppRole> roleManager,
     IMapper mapper,
     IConfiguration config) : base(mapper, config, currentUserService)
    {
        _sellerRepo = sellerRepo;
        _shopRepo = shopRepo;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    #region Create Seller Application/Registration
    public async Task<IDataResult<Seller>> CreateSellerApplicationAsync(RegisterSellerViewModel model)
    {
        #region Existing Control
        var phoneExists = await _sellerRepo.ExistsAsync(s => s.Phone == model.Phone && !s.IsDeleted);

        if (phoneExists)
        {
            return new ErrorDataResult<Seller>(Messages.AlreadyExistsPhone);
        }

        var taxNumberExists = await _shopRepo.ExistsAsync(s => s.TaxNumber.Trim() == model.TaxNumber.Trim() && !s.IsDeleted);

        if (taxNumberExists)
        {
            return new ErrorDataResult<Seller>(Messages.AlreadyExistsTaxNumber);
        }

        var shopNameExists = await _shopRepo.ExistsAsync(s => s.Name.ToLower().Trim() == model.ShopName.ToLower().Trim() && !s.IsDeleted);

        if (shopNameExists)
        {
            return new ErrorDataResult<Seller>(Messages.AlreadyExistsShopName);
        }
        #endregion

        using var transaction = await _sellerRepo.BeginTransactionAsync();

        // Create Appuser
        var createAppUserResult = await CreateAppUserAsync(model);

        if (!createAppUserResult.Success)
        {
            return new ErrorDataResult<Seller>(message: createAppUserResult.Message);
        }

        // Create Seller
        var userId = createAppUserResult.Data.Id;
        var createSellerResult = await CreateSellerAsync(model, userId);

        if (!createSellerResult.Success)
        {
            return new ErrorDataResult<Seller>(message: createSellerResult.Message);
        }

        // Create Shop
        var sellerId = createSellerResult.Data.Id;
        var createShopResult = await CreateShopAsync(model, sellerId);

        if (!createShopResult.Success)
        {
            return new ErrorDataResult<Seller>(message: createShopResult.Message);
        }

        await transaction.CommitAsync();

        return new SuccessDataResult<Seller>();
    }
    #endregion

    #region Private Methods
    private async Task<DataResult<AppUser>> CreateAppUserAsync(RegisterSellerViewModel model)
    {
        var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.Phone);

        if (existingUser != null && !existingUser.IsDeleted)
        {
            return new ErrorDataResult<AppUser>(message: Messages.AlreadyExistsPhone);
        }

        var user = new AppUser
        {
            Email = model.Email,
            UserName = model.Email,
            PhoneNumber = model.Phone
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errorMessages = string.Join(" | ", result.Errors.Select(e => e.Description));
            return new ErrorDataResult<AppUser>(message: errorMessages);
        }

        if (!await _roleManager.RoleExistsAsync(CustomRoles.Seller))
            await _roleManager.CreateAsync(new AppRole { Name = CustomRoles.Seller });

        await _userManager.AddToRoleAsync(user, CustomRoles.Seller);

        return new SuccessDataResult<AppUser>(data: user);
    }

    private async Task<DataResult<Seller>> CreateSellerAsync(RegisterSellerViewModel model, Guid userId)
    {
        var existingSeller = await _sellerRepo.GetByIdAsync(userId);

        if (existingSeller != null)
        {
            return new ErrorDataResult<Seller>(Messages.AlreadyExistsSeller);
        }

        var seller = Mapper.Map<Seller>(model);

        seller.Id = userId;
        seller.CreatedBy = model.FullName;
        seller.IsActive = false;

        var createSellerResult = await _sellerRepo.CreateAsync(seller);

        if (createSellerResult <= 0)
        {
            return new ErrorDataResult<Seller>(message: Messages.CreateError);
        }

        return new SuccessDataResult<Seller>(data: seller);
    }

    private async Task<DataResult<Shop>> CreateShopAsync(RegisterSellerViewModel model, Guid sellerId)
    {
        var existingShop = await _shopRepo.ExistsAsync(s => s.SellerId == sellerId && !s.IsDeleted);

        if (existingShop)
        {
            return new ErrorDataResult<Shop>(message: Messages.AlreadyExistsShop);
        }

        var shop = Mapper.Map<Shop>(model);

        shop.SellerId = sellerId;
        shop.CreatedBy = model.FullName;
        shop.IsActive = false;

        var createShopResult = await _shopRepo.CreateAsync(shop);
        if (createShopResult <= 0)
        {
            return new ErrorDataResult<Shop>(message: Messages.CreateError);
        }

        return new SuccessDataResult<Shop>(data: shop);
    }
 
    #endregion

     
   public async Task<IDataResult<Seller>> GetActiveSellerByUserIdAsync(Guid userId)
    {
        var seller = await _sellerRepo.GetActiveSellerByUserIdAsync(userId);

        if (seller == null)
            return new ErrorDataResult<Seller>(message: Messages.SellerNotFound);

        return new SuccessDataResult<Seller>(data: seller);
    }
}