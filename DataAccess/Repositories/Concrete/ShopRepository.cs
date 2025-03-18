using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete
{
    public class ShopRepository(ApplicationDbContext context) :BaseRepository<Shop>(context), IShopRepository
    {
        public async Task<Shop?> GetShopBySellerIdAsync(Guid sellerId)
        {
            var shop = await _dataContext.Shops
        .AsNoTracking()  
        .FirstOrDefaultAsync(s => s.SellerId == sellerId);
            return shop;
        }
    }
}
