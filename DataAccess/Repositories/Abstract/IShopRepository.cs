using Models.Entities.Concrete;

namespace DataAccess.Repositories.Abstract
{
    public interface IShopRepository : IBaseRepository<Shop>
    {
        Task<Shop?> GetShopBySellerIdAsync(Guid sellerId);
    }
}
