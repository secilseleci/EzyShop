using Core.Constants;
using Core.Pagination;
using Core.Utilities.Results;
using Models.DTOs;
using Models.Entities.Concrete;
namespace Business.Services.Abstract;

public interface IShopService
{
    Task<IDataResult<PaginatedList<ShopListDto>>> GetShopsAsync(ShopStatus status, string? searchTerm, int page, int pageSize);
    Task<IDataResult<ShopDetailsDto>> GetShopDetailsAsync(Guid shopId);
    Task<IResult>ApproveShopAsync(Guid shopId, Guid sellerId);
    Task<IResult> RejectShopAsync(Guid shopId, Guid sellerId);
    Task<IResult> DeactivateShopAsync(Guid shopId, Guid sellerId);
    Task<IResult> ReactivateShopAsync(Guid shopId, Guid sellerId);
    Task<IResult> DeleteShopAsync(Guid shopId, Guid sellerId);

    Task<int> CountPendingShopsAsync();
    Task<int> CountActiveShopsAsync();

    Task<IDataResult<Shop>> GetActiveShopBySellerIdAsync(Guid sellerId);
}
