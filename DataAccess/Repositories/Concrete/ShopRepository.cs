using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete
{
    public class ShopRepository(ApplicationDbContext context) :BaseRepository<Shop>(context), IShopRepository
    {
        public async Task<Shop?> GetBySellerIdAsync(Guid sellerId)
        {
            return await _dataContext.Shops
                .FirstOrDefaultAsync(s => s.SellerId == sellerId);
        }
    }
}
