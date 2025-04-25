using Core.Pagination;
using Models.DTOs;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Abstract;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<PaginatedList<ProductListDto>> GetProductDtosAsync(Guid currentShopId,string? searchTerm, int page, int pageSize);

}
