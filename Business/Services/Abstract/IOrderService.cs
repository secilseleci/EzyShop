using Core.Utilities.Results;
using Models.ViewModels;

namespace Business.Services.Abstract
{
    public interface IOrderService
    {
        Task<IDataResult<SummaryViewModel>> GetOrderSummaryAsync(Guid userId);
        Task<IDataResult<List<string>>> CreateOrderAsync(SummaryViewModel model);
    }
}
