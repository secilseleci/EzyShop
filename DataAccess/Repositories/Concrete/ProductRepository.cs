using Core.Pagination;
using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
using System.Linq.Expressions;
using static Models.Entities.Concrete.Shop;

namespace DataAccess.Repositories.Concrete;

public class ProductRepository(ApplicationDbContext context) : BaseRepository<Product>(context), IProductRepository
{
    public async Task<Product?> GetProductWithIncludesAsync(Guid id)
    {
        return await _dataContext.Products
            .Include(p => p.Category)
            .Include(p => p.Shop).ThenInclude(s => s.Seller)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }
    public async Task<IEnumerable<Product>> GetAllProductsWithIncludesAsync(Expression<Func<Product, bool>>? predicate = null)
    {
        var query = _dataContext.Products
            .Include(p => p.Category)
            .Include(p => p.Shop)
            .Include(p => p.ProductImages)
            .Where(p => !p.IsDeleted)
            .Where(p => p.Shop.Status == ShopStatus.Approved)
            .AsQueryable();

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.ToListAsync();
    }
    public async Task<PaginatedList<Product>> GetPaginatedForCustomerAsync(Expression<Func<Product, bool>> predicate, int page, int pageSize)
    {
        var query = _dataContext.Products
            .AsNoTracking()
            .Where(predicate)
            .Where(p => !p.IsDeleted && p.IsActive && p.Stock > 0 && p.Shop.Status == ShopStatus.Approved);

        var totalItems = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<Product>(items, totalItems, page, pageSize);
    }

    public async Task<PaginatedList<Product>> GetPaginatedForSellerAsync(Guid shopId, int page, int pageSize)
    {
        var query = _dataContext.Products
            .AsNoTracking()
            .Where(p => p.ShopId == shopId && !p.IsDeleted);

        var totalItems = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<Product>(items, totalItems, page, pageSize);
    }

    public async Task<PaginatedList<Product>> GetFilteredPaginatedProductsAsync(
        string? name,
        string? category,
        string? color,
        decimal? minPrice,
        decimal? maxPrice,
        int page,
        int pageSize)
    {
        var query = _dataContext.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => !p.IsDeleted && p.IsActive && p.Stock > 0 && p.Shop.Status == ShopStatus.Approved);

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.ToLower().Contains(name.Trim().ToLower()));

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(p => p.Category.Name.ToLower() == category.Trim().ToLower());

        if (!string.IsNullOrWhiteSpace(color))
            query = query.Where(p => p.Color != null && p.Color.ToLower() == color.Trim().ToLower());

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        var totalItems = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<Product>(items, totalItems, page, pageSize);
    }
}
