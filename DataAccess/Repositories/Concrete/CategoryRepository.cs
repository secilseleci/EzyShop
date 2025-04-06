using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;


namespace DataAccess.Repositories.Concrete;

public class CategoryRepository(ApplicationDbContext context) :BaseRepository<Category>(context), ICategoryRepository
{
    
    public async Task<Category?> GetCategoryWithProductsAsync(Guid categoryId)
    {
        return await _dataContext.Categories
              .Include(c => c.Products)
              .FirstOrDefaultAsync(c => c.Id == categoryId && !c.IsDeleted);
    }
 
}
