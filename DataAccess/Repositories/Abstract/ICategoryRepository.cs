using Models.Entities.Concrete;

namespace DataAccess.Repositories.Abstract
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category?> GetCategoryWithProductsAsync(Guid categoryId);
        Task<int> GetMaxDisplayOrderAsync();

    }
}
