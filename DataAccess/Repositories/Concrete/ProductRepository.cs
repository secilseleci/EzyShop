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
    }
}
