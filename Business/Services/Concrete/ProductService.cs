using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;
using Models.ViewModels.Product;
using System.Linq.Expressions;

namespace Business.Services.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        #region Create
        public async Task<IResult> CreateProductAsync(ProductViewModel model)
        {
            if (model.ShopId == Guid.Empty)
            {
                return new ErrorResult("Invalid shop information. Please try again.");
            }
            var product = _mapper.Map<Product>(model);
 

            var addResult = await _productRepository.CreateAsync(product);
            return addResult > 0
                ? new SuccessResult(Messages.CreateProductSuccess)
                : new ErrorResult(Messages.CreateProductError);
        }
        #endregion

        #region Update
        public async Task<IResult> UpdateProductAsync(ProductViewModel model  )
        {
            var productResult = await GetProductByIdAsync(model.Id);
            if (!productResult.Success)
            {
                return productResult;
            }

            CompleteUpdate(model, productResult);
            return await GetUpdateResultAsync(productResult);

        }
        private async Task<IResult> GetUpdateResultAsync(IDataResult<Product> productResult)
        {
            var updateResult = await _productRepository.UpdateAsync(productResult.Data);
            return updateResult > 0
                ? new SuccessResult(Messages.UpdateProductSuccess)
                : new ErrorResult(Messages.UpdateProductError);
        }
        #endregion

        #region Delete
        public async Task<IResult> DeleteProductAsync(Guid productId )
        {

            var deleteProductResult = await _productRepository.DeleteAsync(productId);
            return deleteProductResult > 0
                ? new SuccessResult(Messages.DeleteProductSuccess)
                : new ErrorResult(Messages.DeleteProductError);
        }
        #endregion

        #region Read
        public async Task<IDataResult<Product>> GetProductByIdAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            return product is not null
                ? new SuccessDataResult<Product>(product)
                : new ErrorDataResult<Product>(Messages.ProductNotFound);
        }

        public async Task<IDataResult<IEnumerable<Product>>> GetAllProductsAsync(
            Expression<Func<Product, bool>> predicate )
        {
            var productList = await _productRepository.GetAllAsync(predicate);
            return productList is not null && productList.Any()
                ? new SuccessDataResult<IEnumerable<Product>>(productList)
                : new ErrorDataResult<IEnumerable<Product>>(Messages.EmptyProductList);
        }

        public async Task<IDataResult<Product>> GetProductByIdWithCategoryAsync(Guid productId)
        {
            var product = await _productRepository.GetProductWithCategoryAsync(productId);
            return product is not null
               ? new SuccessDataResult<Product>(product)
               : new ErrorDataResult<Product>(Messages.ProductNotFound);
        }

        public async Task<IDataResult<IEnumerable<Product>>> GetAllProductsWithCategoryAsync(Expression<Func<Product, bool>> predicate)
        {
            var productList = await _productRepository.GetAllProductsWithCategoryAsync(predicate);
            return productList is not null && productList.Any()
                ? new SuccessDataResult<IEnumerable<Product>>(productList)
                : new ErrorDataResult<IEnumerable<Product>>(Messages.EmptyProductList);
        }

        public async Task<IDataResult<IEnumerable<ProductViewModel>>> GetFilteredProductsAsync(
       string? name, string? category, string? color, decimal? minPrice, decimal? maxPrice)
        {
            var products = await _productRepository.GetFilteredProductsAsync(name, category, color, minPrice, maxPrice);

            if (!products.Any())
            {
                return new ErrorDataResult<IEnumerable<ProductViewModel>>(Messages.NoProductFilters);
            }

            var mappedProducts = _mapper.Map<IEnumerable<ProductViewModel>>(products);
            return new SuccessDataResult<IEnumerable<ProductViewModel>>(mappedProducts);
        }
        #endregion

        #region Helper Methods
        private static void CompleteUpdate(ProductViewModel model, IDataResult<Product> productResult)
        {
            productResult.Data.Name = model.Name;
            productResult.Data.Description = model.Description;
            productResult.Data.Color = model.Color;
            productResult.Data.Price = model.Price;
            productResult.Data.Stock = model.Stock;
            productResult.Data.CategoryId = model.CategoryId;
            productResult.Data.ImageUrl = model.ImageUrl ?? productResult.Data.ImageUrl;
        }
        #endregion
    }
}
