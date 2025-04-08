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

    #region Read
    Task<IDataResult<PaginatedList<SellerApplicationViewModel>>> GetPaginatedAllApplicationsAsync(int page, int pageSize);
    Task<IDataResult<PaginatedList<SellerApplicationViewModel>>> GetPaginatedApplicationsByStatusAsync(ApplicationStatus status, int page, int pageSize);
    #endregion

    Task<IResult> ApproveSellerAsync(Guid id);
    Task<IResult> RejectSellerAsync(Guid id);
   
}
