using Core.Pagination;
using Core.Utilities.Results;
using Models.DTOs;
using Models.ViewModels.Product;

namespace Business.Services.Abstract;

public interface IProductService
{
    Task<IDataResult<PaginatedList<ProductListDto>>> GetProductsAsync(Guid currentShopId, string? searchTerm, int page, int pageSize);

    Task<IResult> CreateProductAsync(CreateProductViewModel model);
}
