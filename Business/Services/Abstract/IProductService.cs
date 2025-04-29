using Core.Constants;
using Core.Pagination;
using Core.Utilities.Results;
using Models.DTOs;
using Models.Entities.Concrete;
using Models.ViewModels.Product;

namespace Business.Services.Abstract;

public interface IProductService
{
    Task<IDataResult<PaginatedList<ProductListDto>>> GetProductsAsync(ProductStatus status, Guid userId, string? searchTerm, int page, int pageSize);
    Task<IDataResult<ProductDetailsDto>> GetProductDetailsAsync(Guid userId, Guid productId);
    Task<IResult> CreateProductAsync(CreateProductViewModel model, Guid userId);
    Task<IResult> UpdateProductAsync(UpdateProductViewModel model, Guid userId);
    Task<IResult> DeactivateProductAsync(Guid productId, Guid userId);
    Task<IResult> ReactivateProductAsync(Guid productId, Guid userId, int stock);
    Task<IDataResult<Product>> GetProductByIdAsync(Guid productId);
    Task<IResult> DeleteProductAsync(Guid productId, Guid userId);
}
