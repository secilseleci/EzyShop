using AutoMapper;
using Business.Services.Abstract;
using Business.Services.Concrete;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;
using Models.ViewModels;
using System.Linq.Expressions;

public class ShopService : IShopService
{
    private readonly IShopRepository _shopRepository;
    private readonly IOrderRepository _orderRepository;

    private readonly IMapper _mapper;

    public ShopService(IShopRepository shopRepository,IOrderRepository orderRepository, IMapper mapper)
    {
        _shopRepository = shopRepository;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    #region Read
    public async Task<IDataResult<Shop>> GetShopByIdAsync(Guid shopId)
    {
        var shop = await _shopRepository.GetByIdAsync(shopId);
        return shop is not null
            ? new SuccessDataResult<Shop>(shop)
            : new ErrorDataResult<Shop>(Messages.ShopNotFound);
    }

    public async Task<IDataResult<IEnumerable<Shop>>> GetAllShopsAsync(
        Expression<Func<Shop, bool>> predicate)
    {
        var shopList = await _shopRepository.GetAllAsync(predicate);
        return shopList is not null && shopList.Any()
            ? new SuccessDataResult<IEnumerable<Shop>>(shopList)
            : new ErrorDataResult<IEnumerable<Shop>>(Messages.EmptyShopList);
    }

    public async Task<IDataResult<ShopViewModel>> GetShopBySellerIdAsync(Guid sellerId)
    {
        var shop = await _shopRepository.GetBySellerIdAsync(sellerId);
        if (shop == null)
        {
            return new ErrorDataResult<ShopViewModel>(Messages.ShopNotFound);
        }

        var mappedShop = _mapper.Map<ShopViewModel>(shop);
        return new SuccessDataResult<ShopViewModel>(mappedShop);
    }
    #endregion

    #region Create
    public async Task<IResult> CreateShopAsync(Shop entity)
    {
        var existingShop = await GetShopBySellerIdAsync(entity.SellerId);
        if (existingShop != null)
        {
            return new ErrorResult(Messages.ShopAlreadyExists);
        }


        var result = await _shopRepository.CreateAsync(entity);
        return result > 0
            ? new SuccessResult(Messages.CreateShopSuccess)
            : new ErrorResult(Messages.CreateShopError);
    }
    #endregion
    #region Delete
    public async Task<IResult> DeleteShopAsync(Guid shopId)
    {
 
        var deleteShopResult = await _shopRepository.DeleteAsync(shopId);
        return deleteShopResult > 0
            ? new SuccessResult(Messages.DeleteShopSuccess)
            : new ErrorResult(Messages.DeleteShopError);
    }
    #endregion
    #region Update
    public async Task<IResult> UpdateShopAsync(ShopViewModel model)
    {
        var shopResult = await GetShopByIdAsync(model.Id);
        if (!shopResult.Success)
        {
            return shopResult;
        }
        if (IsShopDataSame(shopResult.Data, model))
        {
            return new ErrorResult("No changes detected.");
        }
        CompleteUpdate(model, shopResult);
        return await GetUpdateResultAsync(shopResult);

    }
  
    #endregion
    #region Helper Methods
  private async Task<IResult> GetUpdateResultAsync(IDataResult<Shop> shopResult)
    {
        var updateResult = await _shopRepository.UpdateAsync(shopResult.Data);
        return updateResult > 0
            ? new SuccessResult(Messages.UpdateShopSuccess)
            : new ErrorResult(Messages.UpdateShopError);
    }
    private static bool IsShopDataSame(Shop shop, ShopViewModel model)
    {
        return shop.ContactNumber == model.ContactNumber && shop.Address == model.Address;
    }
    private static void CompleteUpdate(ShopViewModel model, IDataResult<Shop> shopResult)
    {
        shopResult.Data.ContactNumber = model.ContactNumber;
        shopResult.Data.Address = model.Address;
    }
    #endregion
}
