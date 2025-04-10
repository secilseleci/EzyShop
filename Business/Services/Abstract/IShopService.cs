using Core.Pagination;
using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels.Shop;

namespace Business.Services.Abstract;

public interface IShopService
{
    Task<IResult> CheckShopIsActiveAsync(Guid userId);

    Task<IResult> CreateShopAsync(Shop entity);
 
    Task<IDataResult<Shop>> GetShopByIdAsync(Guid shopId);
    Task<IDataResult<ShopViewModel>> GetShopBySellerIdAsync(Guid sellerId);
    Task<IDataResult<ShopViewModel>> GetShopByUserIdAsync(Guid userId);


    Task<IDataResult<PaginatedList<ShopViewModel>>> GetPaginatedShopsAsync(int page, int pageSize);
    Task<IDataResult<PaginatedList<ShopViewModel>>> GetPaginatedShopsByStatusAsync(Shop.ShopStatus status, int page, int pageSize);
    Task<IResult> DeleteShopAsync(Guid shopId);
}
