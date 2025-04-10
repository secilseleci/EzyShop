using Core.Pagination;
using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete;

public class ShopRepository(ApplicationDbContext context) : BaseRepository<Shop>(context), IShopRepository
{
    public async Task<Shop?> GetShopByUserIdAsync(Guid userId)
    {
        return await _dataContext.Shops
            .Include(s => s.Seller)
            .FirstOrDefaultAsync(s => s.Seller.UserId == userId && !s.IsDeleted);
    }

    public async Task<Shop?> GetShopBySellerIdAsync(Guid sellerId) =>
    await _dataContext.Shops
        .AsNoTracking()
        .FirstOrDefaultAsync(s => s.SellerId == sellerId && !s.IsDeleted);


    public async Task<PaginatedList<Shop>> GetPaginatedShopsAsync(int page, int pageSize)
    {
        var query = _dataContext.Shops
            .AsNoTracking()
            .Where(s => !s.IsDeleted);

        var totalItems = await query.CountAsync();
        var items = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<Shop>(items, totalItems, page, pageSize);
    }


    public async Task<PaginatedList<Shop>> GetPaginatedShopsByStatusAsync(Shop.ShopStatus status, int page, int pageSize)
    {
        var query = _dataContext.Shops
            .AsNoTracking()
            .Where(s => !s.IsDeleted && s.Status == status);

        var totalItems = await query.CountAsync();
        var items = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<Shop>(items, totalItems, page, pageSize);
    }
}
