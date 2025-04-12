using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Pagination;
using Core.Security;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.Extensions.Configuration;
using Models.Entities.Concrete;
using Models.ViewModels.Shop;
using static Models.Entities.Concrete.Shop;

namespace Business.Services.Concrete;
public class ShopService : BaseService,IShopService
{
    private readonly IShopRepository _shopRepo;
    private readonly IProductRepository _productRepo;
    private readonly ISellerRepository _sellerRepo;
    public ShopService(
        IShopRepository shopRepo, 
        IProductRepository productRepo,
        ISellerRepository sellerRepo,
        IMapper mapper,
        IConfiguration config,
        ICurrentUserService currentUser) : base(mapper, config, currentUser)
    {
        _shopRepo = shopRepo;
        _productRepo = productRepo;
        _sellerRepo=sellerRepo;
    }
    public async Task<IResult> CheckShopIsActiveAsync(Guid userId)
    {
        var shop = await _shopRepo.GetShopByUserIdAsync(userId);

        if (shop == null)
            return new ErrorResult(Messages.ShopNotFound);

        if (shop.Status != ShopStatus.Approved)
            return new ErrorResult(Messages.ShopInActive);

        return new SuccessResult();
    }

    #region Create
    public async Task<IResult> CreateShopAsync(Shop entity)
    {
        var shopResult = await GetShopBySellerIdAsync(entity.SellerId);

        if (shopResult.Success && shopResult.Data != null)
            return new ErrorResult(Messages.ShopAlreadyExists);
         
        var result = await _shopRepo.CreateAsync(entity);

        return result > 0
            ? new SuccessResult(Messages.CreateShopSuccess)
            : new ErrorResult(Messages.CreateShopError);
    }
    #endregion

    #region Read
    public async Task<IDataResult<Shop>> GetShopByIdAsync(Guid shopId)
    {
        var shop = await _shopRepo.GetByIdAsync(shopId);
        return shop is not null
            ? new SuccessDataResult<Shop>(shop)
            : new ErrorDataResult<Shop>(Messages.ShopNotFound);
    }

    public async Task<IDataResult<ShopViewModel>> GetShopBySellerIdAsync(Guid sellerId)
    {
        var shop = await _shopRepo.GetShopBySellerIdAsync(sellerId);
        if (shop == null)
            return new ErrorDataResult<ShopViewModel>(Messages.ShopNotFound);
         
        var mappedShop = Mapper.Map<ShopViewModel>(shop);
        return new SuccessDataResult<ShopViewModel>(mappedShop);
    }
    public async Task<IDataResult<ShopViewModel>> GetShopByUserIdAsync(Guid userId)
    {
        var shop = await _shopRepo.GetShopByUserIdAsync(userId);
        if (shop == null)
            return new ErrorDataResult<ShopViewModel>(Messages.ShopNotFound);

        var mappedShop = Mapper.Map<ShopViewModel>(shop);
        return new SuccessDataResult<ShopViewModel>(mappedShop);
    }

    public async Task<IDataResult<PaginatedList<ShopViewModel>>> GetPaginatedShopsAsync(int page, int pageSize)
    {
        var pagedShops = await _shopRepo.GetPaginatedAsync(s => !s.IsDeleted, page, pageSize);

        var viewModels = Mapper.Map<IEnumerable<ShopViewModel>>(pagedShops.Items);

        var result = new PaginatedList<ShopViewModel>(
            viewModels,
            pagedShops.TotalItems,
            pagedShops.Page,
            pagedShops.PageSize
        );

        return result.Items.Any()
            ? new SuccessDataResult<PaginatedList<ShopViewModel>>(result)
            : new ErrorDataResult<PaginatedList<ShopViewModel>>(Messages.EmptyShopList);
    }

    public async Task<IDataResult<PaginatedList<ShopViewModel>>> GetPaginatedShopsByStatusAsync(Shop.ShopStatus status, int page, int pageSize)
    {
        var pagedShops = await _shopRepo.GetPaginatedAsync(s => !s.IsDeleted && s.Status == status, page, pageSize);

        var viewModels = Mapper.Map<IEnumerable<ShopViewModel>>(pagedShops.Items);

        var result = new PaginatedList<ShopViewModel>(
            viewModels,
            pagedShops.TotalItems,
            pagedShops.Page,
            pagedShops.PageSize
        );

        return result.Items.Any()
            ? new SuccessDataResult<PaginatedList<ShopViewModel>>(result)
            : new ErrorDataResult<PaginatedList<ShopViewModel>>(Messages.EmptyShopList);
    }

    #endregion

    #region Delete
    public async Task<IResult> DeleteShopAsync(Guid shopId)
    {
        var shop = await _shopRepo.GetByIdAsync(shopId);
        if (shop is null)
            return new ErrorResult(Messages.ShopNotFound);

        using var transaction = await _shopRepo.BeginTransactionAsync();

        try
        {
            // Seller
            var seller = await _sellerRepo.GetByIdAsync(shop.SellerId);
            if (seller is not null)
                await _sellerRepo.SoftDeleteAsync(seller.Id);

            // Products
            var products = await _productRepo.GetWhereAsync(p => p.ShopId == shopId);
            if (products is not null && products.Any())
                await _productRepo.SoftDeleteRangeAsync(products);

            // Shop
            var result = await _shopRepo.SoftDeleteAsync(shopId);

            if (result <= 0)
            {
                await transaction.RollbackAsync();
                return new ErrorResult(Messages.DeleteShopError);
            }

            await transaction.CommitAsync();
            return new SuccessResult(Messages.DeleteShopSuccess);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return new ErrorResult(Messages.DeleteShopError);
        }
    }

  

    #endregion


}
