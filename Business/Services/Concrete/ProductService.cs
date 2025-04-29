using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Interfaces;
using Core.Pagination;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models.DTOs;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels.Product;

namespace Business.Services.Concrete;

public class ProductService : BaseService, IProductService
{
    private readonly IShopService _shopService;
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;

    public ProductService(
      IMapper mapper,
      IConfiguration config,
      UserManager<AppUser> userManager,
      RoleManager<AppRole> roleManager,
      ICurrentUserService currentUserService,
      IProductRepository productRepo,
      ICategoryRepository categoryRepo,
        IShopService shopService) : base(mapper, config, currentUserService)
    {
        _productRepo = productRepo;
        _shopService = shopService;
        _categoryRepo = categoryRepo;
    }

    public async Task<IResult> CreateProductAsync(CreateProductViewModel model, Guid userId)
    {
        var shopId = await _shopService.GetActiveShopIdByUserIdAsync(userId);
        if (!shopId.Success)
            return new ErrorResult(Messages.ShopNotFound);

        var categoryExists = await _categoryRepo.ExistsAsync(c => c.Id == model.CategoryId && !c.IsDeleted);
        if (!categoryExists)
            return new ErrorResult(Messages.CategoryNotFound);


        var product = Mapper.Map<Product>(model);
        product.ShopId = shopId.Data;

        var createResult = await _productRepo.CreateAsync(product);

        return createResult <= 0
             ? new ErrorResult(message: Messages.CreateError)
             : new SuccessResult(message: Messages.CreateSuccess);
    }

    public async Task<IDataResult<PaginatedList<ProductListDto>>> GetProductsAsync(ProductStatus status, Guid userId, string? searchTerm, int page, int pageSize)
    {
        var shopId = await _shopService.GetActiveShopIdByUserIdAsync(userId);

        if (!shopId.Success)
            return new ErrorDataResult<PaginatedList<ProductListDto>>(Messages.ShopNotFound);

        var result = await _productRepo.GetProductDtosAsync(status, shopId.Data, searchTerm, page, pageSize);

        return new SuccessDataResult<PaginatedList<ProductListDto>>(result);
    }

    public async Task<IDataResult<ProductDetailsDto>> GetProductDetailsAsync(Guid userId, Guid productId)
    {
        var shopId = await _shopService.GetActiveShopIdByUserIdAsync(userId);
        if (!shopId.Success)
            return new ErrorDataResult<ProductDetailsDto>(Messages.ShopNotFound);

        var result = await _productRepo.GetProductDetailsDtosAsync(shopId.Data, productId);

        if (result == null)
        {
            return new ErrorDataResult<ProductDetailsDto>(message: Messages.ProductNotFound);
        }

        return new SuccessDataResult<ProductDetailsDto>(data: result);
    }

    public async Task<IResult> DeactivateProductAsync(Guid productId, Guid userId)
    {
        if (!await IsAvailable(productId))
        { return new ErrorResult(Messages.ProductAlreadyInactive); }

        var product = await _productRepo.GetByIdAsync(productId);
        if (product is null) return new ErrorResult(Messages.ProductNotFound);

        product.Stock = 0;
        product.IsActive = false;
        var result = await _productRepo.UpdateAsync(product);

        return result > 0
            ? new SuccessResult(Messages.DeactivateProductSuccess)
             : new ErrorResult(Messages.DeactivateProductError);
    }

    public async Task<IResult> ReactivateProductAsync(Guid productId, Guid userId, int stock)
    {
        if (await IsAvailable(productId))
        { return new ErrorResult(Messages.ProductAlreadyActive); }

        var product = await _productRepo.GetByIdAsync(productId);
        if (product is null) return new ErrorResult(Messages.ProductNotFound);

        product.Stock = stock;
        product.IsActive = true;
        var result = await _productRepo.UpdateAsync(product);

        return result > 0
            ? new SuccessResult(Messages.ReactivateProductSuccess)
             : new ErrorResult(Messages.ReactivateProductError);
    }
    private async Task<bool> IsAvailable(Guid productId)
    {
        var product = await _productRepo.GetByIdAsync(productId);

        if (product == null)
            return false;

        if (!product.IsActive)
            return false;

        if (product.Stock <= 0)
            return false;

        return true;
    }

    public async Task<IResult> UpdateProductAsync(UpdateProductViewModel model, Guid userId)
    {
        var shopId = await _shopService.GetActiveShopIdByUserIdAsync(userId);
        if (!shopId.Success)
            return new ErrorResult(Messages.ShopNotFound);

        var categoryExists = await _categoryRepo.ExistsAsync(c => c.Id == model.CategoryId && !c.IsDeleted);
        if (!categoryExists)
            return new ErrorResult(Messages.CategoryNotFound);

        var existingProduct = await GetProductByIdAsync(model.Id);
        if (existingProduct is null)
            return new ErrorDataResult<Product>(Messages.ProductNotFound);

        existingProduct.Data.Name = model.Name;
        existingProduct.Data.Price = model.Price;
        existingProduct.Data.Stock = model.Stock;
        existingProduct.Data.Color = model.Color;
        existingProduct.Data.CategoryId = model.CategoryId;
        existingProduct.Data.ImageUrl = model.ImageUrl;

        var updateResult = await _productRepo.UpdateAsync(existingProduct.Data);

        return updateResult > 0
            ? new SuccessResult(Messages.UpdateSuccess)
            : new ErrorResult(Messages.UpdateError);
    }

    public async Task<IDataResult<Product>> GetProductByIdAsync(Guid productId)
    {
        var product = await _productRepo.GetByIdAsync(productId);

        if (product is null)
            return new ErrorDataResult<Product>(Messages.ProductNotFound);

        return new SuccessDataResult<Product>(product);
    }

    public async Task<IResult> DeleteProductAsync(Guid productId, Guid userId)
    {
        var shopId = await _shopService.GetActiveShopIdByUserIdAsync(userId);
        if (!shopId.Success)
            return new ErrorResult(Messages.ShopNotFound);

        if (await IsAvailable(productId))
            return new ErrorResult(Messages.DeleteProductError);


        var result = await _productRepo.SoftDeleteAsync(productId);
        return result > 0
            ? new SuccessResult(Messages.DeleteSuccess)
             : new ErrorResult(Messages.DeleteError);

    }
}