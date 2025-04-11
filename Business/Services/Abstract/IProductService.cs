using Core.Pagination;
using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels.Product;

namespace Business.Services.Abstract;

public interface IProductService
{
    #region Crud
    Task<IResult> CreateProductAsync(ProductCreateViewModel model, Guid userId);
    Task<IResult> UpdateProductAsync(ProductUpdateViewModel model, Guid userId);
    Task<IResult> DeleteProductAsync(Guid productId);
     Task<IResult> ToggleProductStatusAsync(Guid productId, Guid userId);
    Task<IDataResult<Product>> GetProductByIdAsync(Guid productId);

    #endregion
    #region Seller
    Task<IDataResult<PaginatedList<ProductSellerViewModel>>> GetPaginatedProductsForSellerAsync(Guid userId, int page, int pageSize, string? searchTerm = null);
    Task<IDataResult<ProductCustomerViewModel>> GetProductWithIncludesByIdAsync(Guid productId);

    #endregion

    #region Customer

    Task<IDataResult<PaginatedList<ProductCustomerViewModel>>> GetFilteredProductsAsync(
    int page, int pageSize, string? name, string? category, string? color, decimal? minPrice, decimal? maxPrice);
    #endregion




}
