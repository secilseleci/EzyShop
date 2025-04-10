using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Pagination;
using Core.Security;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Entities.Concrete;
using Models.ViewModels.Product;
using System.Linq.Expressions;

namespace Business.Services.Concrete;

public class ProductService : BaseService, IProductService
{
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IShopService _shopService;

    public ProductService(
        IProductRepository productRepo,
        ICategoryRepository categoryRepo,
        IShopService shopService,
        IMapper mapper,
        IConfiguration config,
        ICurrentUserService currentUser)
       : base(mapper, config, currentUser)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _shopService = shopService;
    }

    #region Create
    public async Task<IResult> CreateProductAsync(ProductCreateViewModel model, Guid userId)
    {
        var shopCheckResult = await _shopService.CheckShopIsActiveAsync(userId);
        if (!shopCheckResult.Success)
            return shopCheckResult;

        var shop = await _shopService.GetShopByUserIdAsync(userId);
        if (shop == null)
            return new ErrorResult(Messages.ShopNotFound);

        var product = Mapper.Map<Product>(model);
        product.ShopId = shop.Data.Id;


        var addResult = await _productRepo.CreateAsync(product);
        return addResult > 0
            ? new SuccessResult(Messages.CreateProductSuccess)
            : new ErrorResult(Messages.CreateProductError);
    }
    #endregion

    #region Soft Delete
    public async Task<IResult> DeleteProductAsync(Guid productId)
    {
        var exists = await _productRepo.ExistsAsync(p => p.Id == productId && !p.IsDeleted);
        if (!exists)
            return new ErrorResult(Messages.ProductNotFound);

        var result = await _productRepo.SoftDeleteAsync(productId);
        return result > 0
            ? new SuccessResult(Messages.DeleteProductSuccess)
            : new ErrorResult(Messages.DeleteProductError);
    }
    #endregion

    #region Update
    public async Task<IResult> UpdateProductAsync(ProductUpdateViewModel model)
    {
        var product = await _productRepo.GetProductWithIncludesAsync(model.Id);
        if (product is null)
            return new ErrorResult(Messages.ProductNotFound);

        var currentUserId = GetUserId();
        if (currentUserId is null || product.Shop.SellerId != currentUserId)
            return new ErrorResult(Messages.UnauthorizedAccess);

        var categoryExists = await _categoryRepo.ExistsAsync(c => c.Id == model.CategoryId && !c.IsDeleted);
        if (!categoryExists)
            return new ErrorResult(Messages.CategoryNotFound);

        if (product.Name == model.Name &&
             product.Color == model.Color &&
             product.Price == model.Price &&
             product.Stock == model.Stock &&
             product.CategoryId == model.CategoryId &&
             product.ImageUrl == model.ImageUrl &&
             product.IsActive == model.IsActive)
        {
            return new ErrorResult(Messages.NoChangesDetected);
        }

        product = Mapper.Map(model, product);

        var updateResult = await _productRepo.UpdateAsync(product);

        return updateResult > 0
            ? new SuccessResult(Messages.UpdateProductSuccess)
            : new ErrorResult(Messages.UpdateProductError);
    }
    public async Task<IResult> ToggleProductStatusAsync(Guid productId)
    {
        var product = await _productRepo.GetProductWithIncludesAsync(productId);
        if (product is null || product.IsDeleted)
            return new ErrorResult(Messages.ProductNotFound);

        var currentUserId = GetUserId();
        if (currentUserId is null || product.Shop.SellerId != currentUserId)
            return new ErrorResult(Messages.UnauthorizedAccess);

        product.IsActive = !product.IsActive;

        var updateResult = await _productRepo.UpdateAsync(product);

        return updateResult > 0
            ? new SuccessResult(Messages.UpdateProductSuccess)
            : new ErrorResult(Messages.UpdateProductError);
    }

    #endregion

    #region Customer Read
    public async Task<IDataResult<Product>> GetProductByIdAsync(Guid productId)
    {
        var product = await _productRepo.GetByIdAsync(productId);

        if (product is null)
            return new ErrorDataResult<Product>(Messages.ProductNotFound);

        return new SuccessDataResult<Product>(product);
    }
    public async Task<IDataResult<ProductCustomerViewModel>> GetProductWithIncludesByIdAsync(Guid productId)
    {
        var product = await _productRepo.GetProductWithIncludesAsync(productId);

        if (product is null)
            return new ErrorDataResult<ProductCustomerViewModel>(Messages.ProductNotFound);

        var viewModel = Mapper.Map<ProductCustomerViewModel>(product);

        return new SuccessDataResult<ProductCustomerViewModel>(viewModel);
    }

    public async Task<IDataResult<PaginatedList<ProductCustomerViewModel>>> GetPaginatedProductsForCustomerAsync(int page, int pageSize)
    {
        var pagedProducts = await _productRepo.GetPaginatedForCustomerAsync(
            p => p.IsActive && p.Stock > 0,
            page,
            pageSize);


        var viewModels = Mapper.Map<IEnumerable<ProductCustomerViewModel>>(pagedProducts.Items);

        var result = new PaginatedList<ProductCustomerViewModel>(
            viewModels,
            pagedProducts.TotalItems,
            pagedProducts.Page,
            pagedProducts.PageSize);

        return result.Items.Any()
            ? new SuccessDataResult<PaginatedList<ProductCustomerViewModel>>(result)
            : new ErrorDataResult<PaginatedList<ProductCustomerViewModel>>(Messages.ProductNotFound);
    }

    public async Task<IDataResult<PaginatedList<FilteredProductCustomerViewModel>>> GetFilteredPaginatedProductsForCustomerAsync(
    string? name,
    string? category,
    string? color,
    decimal? minPrice,
    decimal? maxPrice,
    int page,
    int pageSize)
    {
        var pagedProducts = await _productRepo.GetFilteredPaginatedProductsAsync(
            name,
            category,
            color,
            minPrice,
            maxPrice,
            page,
            pageSize);

        var viewModels = Mapper.Map<IEnumerable<FilteredProductCustomerViewModel>>(pagedProducts.Items);

        var result = new PaginatedList<FilteredProductCustomerViewModel>(
            viewModels,
            pagedProducts.TotalItems,
            pagedProducts.Page,
            pagedProducts.PageSize);

        return result.Items.Any()
            ? new SuccessDataResult<PaginatedList<FilteredProductCustomerViewModel>>(result)
            : new ErrorDataResult<PaginatedList<FilteredProductCustomerViewModel>>(Messages.ProductNotFound);
    }

    #endregion

    #region Seller Read

    public async Task<IDataResult<PaginatedList<ProductSellerViewModel>>> GetPaginatedProductsForSellerAsync(Guid userId, int page, int pageSize, string? searchTerm = null)
    {
        Expression<Func<Product, bool>> predicate;
        if (!string.IsNullOrWhiteSpace(searchTerm))

        {
            var loweredSearchTerm = searchTerm.ToLower().Trim();

            predicate = p =>
            p.Shop.Seller.UserId == userId && (
                    p.Name.Contains(loweredSearchTerm) ||
                    p.Color!.Contains(loweredSearchTerm)) ||
                    (p.Category != null && (p.Category.Name.ToLower().Contains(loweredSearchTerm)));

        }
        else
        {
            predicate = p => p.Shop.Seller.UserId == userId;
        }

        var paginatedProducts = await _productRepo.GetPaginatedAsync(
               predicate,
               page,
               pageSize,
               q => q
               .Include(p => p.ProductImages)
               .Include(p => p.Category));

        var viewModels = Mapper.Map<IEnumerable<ProductSellerViewModel>>(paginatedProducts.Items);

        var result = new PaginatedList<ProductSellerViewModel>(
            viewModels,
            paginatedProducts.TotalItems,
            paginatedProducts.Page,
            paginatedProducts.PageSize
        );

        return result.Items.Any()
            ? new SuccessDataResult<PaginatedList<ProductSellerViewModel>>(result)
            : new ErrorDataResult<PaginatedList<ProductSellerViewModel>>(Messages.EmptyProductList);


    }
    #endregion


    //    GetDeletedProductsForSellerAsync() → çöp kutusu

    //RecoverProductAsync(Guid id) → soft-delete geri alma

    //SearchProductsForSellerAsync(string query)

    //TopSellingProductsAsync() → dashboard için

    //ProductImageService → dosya yükleme/yenileme için ayrı servis
}
