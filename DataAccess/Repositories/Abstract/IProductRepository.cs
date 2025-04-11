using Models.Entities.Concrete;

namespace DataAccess.Repositories.Abstract;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<Product?> GetProductWithIncludesAsync(Guid id);
    

}
