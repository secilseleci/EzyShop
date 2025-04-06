using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete;

public class SellerRepository(ApplicationDbContext context) : BaseRepository<Seller>(context), ISellerRepository
{
    public async Task<Seller?> GetSellerWithShopAsync(Guid sellerId)
    {
        return await _dataContext.Sellers
            .Include(s => s.Shop)
            .FirstOrDefaultAsync(s => s.Id == sellerId && !s.IsDeleted);
    }

    public async Task<Seller?> GetSellerByUserIdAsync(Guid userId)
    {
        return await _dataContext.Sellers
            .FirstOrDefaultAsync(s => s.UserId == userId && !s.IsDeleted);
    }

    public async Task<bool> HasShopAsync(Guid sellerId)
    {
        return await _dataContext.Shops.AnyAsync(shop => shop.SellerId == sellerId && !shop.IsDeleted);
    }

}

