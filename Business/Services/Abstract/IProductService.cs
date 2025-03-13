using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels.Product;
using System.Linq.Expressions;

namespace Business.Services.Abstract
{
    public interface IProductService
    {
        Task<IDataResult<Product>> GetProductByIdAsync(Guid productId);
        Task<IDataResult<Product>> GetProductByIdWithCategoryAsync(Guid productId);

        Task<IDataResult<IEnumerable<Product>>> GetAllProductsAsync(Expression<Func<Product, bool>> predicate);

        Task<IDataResult<IEnumerable<ProductViewModel>>> GetAllProductsWithCategoryAsync(Expression<Func<Product, bool>> predicate);

        Task<IResult> CreateProductAsync(ProductCreateViewModel model);
        Task<IResult> UpdateProductAsync(ProductUpdateViewModel model);
        Task<IResult> DeleteProductAsync(Guid productId );
        Task<IResult> ToggleProductStatusAsync(Guid productId);

        Task<IDataResult<IEnumerable<ProductViewModel>>> GetFilteredProductsAsync(string? name, string? category, string? color, decimal? minPrice, decimal? maxPrice);
    }
}
