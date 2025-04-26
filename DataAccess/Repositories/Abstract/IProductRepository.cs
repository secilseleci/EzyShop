using Core.Constants;
using Core.Pagination;
using Models.DTOs;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Abstract;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<PaginatedList<ProductListDto>> GetProductDtosAsync(ProductStatus status, Guid currentShopId,string? searchTerm, int page, int pageSize);
    Task<ProductDetailsDto> GetProductDetailsDtosAsync(Guid currentShopId, Guid productId);

}
