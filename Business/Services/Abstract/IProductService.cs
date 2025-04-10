using Core.Pagination;
using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels.Product;

namespace Business.Services.Abstract;

public interface IProductService
{
    #region Crud
    Task<IResult> CreateProductAsync(ProductCreateViewModel model,Guid userId);
    Task<IResult> UpdateProductAsync(ProductUpdateViewModel model);
    Task<IResult> DeleteProductAsync(Guid productId);
    Task<IDataResult<Product>> GetProductByIdAsync(Guid productId);
    #endregion
     
    #region Customer

    Task<IDataResult<ProductCustomerViewModel>> GetProductWithIncludesByIdAsync(Guid productId);
    Task<IDataResult<PaginatedList<ProductCustomerViewModel>>> GetPaginatedProductsForCustomerAsync(int page, int pageSize);
    Task<IDataResult<PaginatedList<FilteredProductCustomerViewModel>>> GetFilteredPaginatedProductsForCustomerAsync(
        string? name,
        string? category,
        string? color,
        decimal? minPrice,
        decimal? maxPrice,
        int page,
        int pageSize);
    #endregion
    
    #region Seller
    Task<IDataResult<PaginatedList<ProductSellerViewModel>>> GetPaginatedProductsForSellerAsync(Guid userId, int page, int pageSize, string? searchTerm = null);
     
    #endregion

    Task<IResult> ToggleProductStatusAsync(Guid productId);

}
