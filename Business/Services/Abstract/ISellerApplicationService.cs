using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels;
using System.Linq.Expressions;

namespace Business.Services.Abstract
{
    public interface ISellerApplicationService
    {
        Task<IResult> ApproveSellerAsync(Guid id);
        Task<IResult> RejectSellerAsync(Guid id);



        #region Crud
        Task<IResult> CreateSellerApplicationAsync(BecomeSellerViewModel model);
        Task<IResult> DeleteSellerApplicationAsync(Guid id);
        Task<IDataResult<SellerApplication>> GetSellerApplicationByIdAsync(Guid id);
        Task<IDataResult<IEnumerable<SellerApplication>>> GetAllSellerApplicationsAsync(Expression<Func<SellerApplication, bool>> predicate);

        #endregion
    }
}
