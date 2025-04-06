using Core.Pagination;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Abstract;

public interface IShopRepository : IBaseRepository<Shop>
{
    Task<Shop?> GetShopBySellerIdAsync(Guid sellerId);
    Task<PaginatedList<Shop>> GetPaginatedShopsAsync(int page, int pageSize);

    Task<PaginatedList<Shop>> GetPaginatedShopsByStatusAsync(Shop.ShopStatus status, int page, int pageSize);
}
