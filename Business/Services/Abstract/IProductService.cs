using Core.Constants;
using Core.Pagination;
using Core.Utilities.Results;
using Models.DTOs;
using Models.Entities.Concrete;
using Models.ViewModels.Product;

namespace Business.Services.Abstract;

public interface IProductService
{
    #region Seller
    Task<IResult> CreateProductAsync(CreateProductViewModel model, Guid userId);
    Task<IResult> UpdateProductAsync(UpdateProductViewModel model, Guid userId);
    Task<IResult> DeleteProductAsync(Guid productId, Guid userId);
    Task<IResult> DeactivateProductAsync(Guid productId, Guid userId);
    Task<IResult> ReactivateProductAsync(Guid productId, Guid userId, int stock);
    Task<IDataResult<PaginatedList<ProductListForSellerDto>>> GetProductsAsync(ProductStatus status, Guid userId, string? searchTerm, int page, int pageSize);
    Task<IDataResult<ProductDetailsForSellerDto>> GetProductDetailsForSellerAsync(Guid userId, Guid productId);
    #endregion


    #region Customer
    Task<IDataResult<PaginatedList<ProductListForCustomerDto>>> GetFilteredProductsAsync(ProductFilterViewModel model);
    Task<IDataResult<ProductDetailsForCustomerDto>> GetProductDetailsForCustomerAsync(Guid userId, Guid productId);
    #endregion  

    Task<IDataResult<Product>> GetProductByIdAsync(Guid productId);

}
