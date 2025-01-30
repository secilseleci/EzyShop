using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
 

namespace DataAccess.Repositories.Concrete
{
    public class CategoryRepository(ApplicationDbContext context) : BaseRepository<Category>(context), ICategoryRepository
    {
        public async Task<Category?> GetCategoryWithProductsAsync(Guid categoryId)
        {
            var category = await _dataContext.Categories
                .Where(c => c.Id == categoryId)
                .Include(c => c.Products)
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<int> GetMaxDisplayOrderAsync()
        {
            var maxOrder = await _dataContext.Categories.MaxAsync(c => (int?)c.DisplayOrder) ?? 0;
            return maxOrder;
        }
    }
}
