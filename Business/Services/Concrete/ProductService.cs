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
    private readonly IProductRepository _productRepo;
    public ProductService(
      IMapper mapper,
      IConfiguration config,
      UserManager<AppUser> userManager,
      RoleManager<AppRole> roleManager,
      ICurrentUserService currentUserService,
      IProductRepository productRepo) : base(mapper, config, currentUserService)
    {
        _productRepo = productRepo;
    }

    public async Task<IResult> CreateProductAsync(CreateProductViewModel model)
    {
        var product = Mapper.Map<Product>(model);

        var createResult = await _productRepo.CreateAsync(product);

        return createResult <= 0
             ? new ErrorResult(message: Messages.CreateError)
             : new SuccessResult(message: Messages.CreateSuccess);
    }

    public async Task<IDataResult<PaginatedList<ProductListDto>>> GetProductsAsync(Guid currentShopId, string? searchTerm, int page, int pageSize)
    {
        var result = await _productRepo.GetProductDtosAsync(currentShopId, searchTerm, page, pageSize);

        return new SuccessDataResult<PaginatedList<ProductListDto>>(result);
    }
}
