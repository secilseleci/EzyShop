using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Concrete
{
    public class ProductRepository(ApplicationDbContext context) : BaseRepository<Product>(context), IProductRepository
    {
        public async Task<Product?> GetProductWithCategoryAsync(Guid id)
        {
            return await _dataContext.Products
                .Include(p => p.Category)
                .Include(p => p.Shop)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>?> GetAllProductsWithCategoryAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _dataContext.Products
                .Where(predicate)
                .Include(p => p.Category)
                .Include(p => p.Shop)
                .Include(p => p.ProductImages)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFilteredProductsAsync(string? name, string? category, string? color, decimal? minPrice, decimal? maxPrice)
        {
            var query = _dataContext.Products
                       .Include(p => p.Category)
                       .AsNoTracking()
                       .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.Name == category);
            }

            if (!string.IsNullOrEmpty(color))
            {
                query = query.Where(p => p.Color == color);
            }
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            var products = await query.ToListAsync();
            Console.WriteLine($"DEBUG: {products.Count} ürün bulundu.");
            return products;
        }
    }
}
