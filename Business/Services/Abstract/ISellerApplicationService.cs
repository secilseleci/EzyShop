using Core.Pagination;
using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels.SellerApplication;

namespace Business.Services.Abstract;

public interface ISellerApplicationService
{  
    #region Crud
    Task<IResult> CreateSellerApplicationAsync(SellerApplicationCreateViewModel model);
    Task<IResult> DeleteSellerApplicationAsync(Guid sellerApplicationId);
    Task<IDataResult<SellerApplication>> GetSellerApplicationByIdAsync(Guid sellerApplicationId);
    #endregion

    #region Application Listing (Admin/API)
    Task<IDataResult<PaginatedList<SellerApplicationViewModel>>> GetPaginatedApplicationsAsync(int page, int pageSize, string? search=null, ApplicationStatus? statusFilter = null);
    #endregion
    #region Application Review Actions
    Task<IResult> ApproveSellerAsync(Guid id);
    Task<IResult> RejectSellerAsync(Guid id);
    #endregion

}
