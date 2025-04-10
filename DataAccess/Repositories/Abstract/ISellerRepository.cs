using Models.Entities.Concrete;

namespace DataAccess.Repositories.Abstract;

public interface ISellerRepository : IBaseRepository<Seller>
{
    Task<Seller?> GetSellerWithIncludesAsync(Guid sellerId);
    Task<Seller?> GetSellerByUserIdAsync(Guid userId);
    Task<bool> HasShopAsync(Guid sellerId);

}
