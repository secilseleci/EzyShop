using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels.User;
using System.Linq.Expressions;

namespace Business.Services.Abstract
{
    public interface ISellerApplicationService
    {
        Task<IResult> ApproveSellerAsync(Guid id);
        Task<IResult> RejectSellerAsync(Guid id);



        #region Crud
        Task<IResult> CreateSellerApplicationAsync(SellerRegistrationViewModel model);
        Task<IDataResult<SellerApplication>> GetSellerApplicationByIdAsync(Guid id);
        Task<IDataResult<IEnumerable<SellerApplication>>> GetAllSellerApplicationsAsync(Expression<Func<SellerApplication, bool>> predicate);

        #endregion
    }
}
