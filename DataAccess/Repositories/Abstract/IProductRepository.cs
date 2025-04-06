using Core.Pagination;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Abstract;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<Product?> GetProductWithIncludesAsync(Guid id);
    Task<PaginatedList<Product>> GetPaginatedForCustomerAsync(Expression<Func<Product, bool>> predicate, int page, int pageSize);
    Task<PaginatedList<Product>> GetPaginatedForSellerAsync(Guid shopId, int page, int pageSize);
    Task<IEnumerable<Product>> GetAllProductsWithIncludesAsync(Expression<Func<Product, bool>>? predicate=null);
    Task<PaginatedList<Product>> GetFilteredPaginatedProductsAsync(
        string? name,
        string? category,
        string? color,
        decimal? minPrice,
        decimal? maxPrice,
        int page,
        int pageSize);

}
