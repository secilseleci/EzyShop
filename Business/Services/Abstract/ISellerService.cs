using Core.Pagination;
using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels.Seller;

namespace Business.Services.Abstract;

public interface ISellerService
{
     
    Task<IResult> DeleteSellerAsync(Guid sellerId);
    Task<IDataResult<PaginatedList<SellerListViewModel>>> GetPaginatedSellersAsync(
        int page,
        int pageSize,
        string? searchTerm = null);

    Task<IDataResult<Seller>> GetSellerByIdAsync(Guid sellerId);

    Task<IDataResult<SellerViewModel>> GetSellerByUserId(Guid userId);

}
