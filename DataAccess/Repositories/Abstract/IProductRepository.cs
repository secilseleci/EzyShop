using Core.Constants;
using Core.Pagination;
using Models.DTOs;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Abstract;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<PaginatedList<ProductListDto>> GetProductDtosAsync(ProductStatus status, Guid shopId, string? searchTerm, int page, int pageSize);
    Task<ProductDetailsDto> GetProductDetailsDtosAsync(Guid shopId, Guid productId);
    Task<PaginatedList<ProductListForCustomersDto>> GetProductForCustomersDtosAsync(string? searchTerm, int page, int pageSize);
}
