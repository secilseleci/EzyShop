using Core.Constants;
using Core.Pagination;
using Core.Utilities.Results;
using Models.DTOs;
using Models.ViewModels.Product;

namespace Business.Services.Abstract;

public interface IProductService
{
    Task<IDataResult<PaginatedList<ProductListDto>>> GetProductsAsync(ProductStatus status, Guid currentShopId, string? searchTerm, int page, int pageSize);
    Task<IDataResult<ProductDetailsDto>> GetProductDetailsAsync(Guid currentShopId, Guid productId);

    Task<IResult> CreateProductAsync(CreateProductViewModel model);
}
