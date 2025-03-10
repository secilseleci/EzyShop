using Core.Utilities.Results;
using Models.ViewModels.Order;

namespace Business.Services.Abstract
{
    public interface IOrderService
    {
        Task<IDataResult<SummaryViewModel>> GetOrderSummaryAsync(Guid userId);
        Task<IDataResult<List<string>>> CreateOrderAsync(SummaryViewModel model);
        Task<IDataResult<IEnumerable<OrderViewModel>>> GetAllOrdersAsync(Guid shopId);
        Task<IDataResult<OrderViewModel>> GetOrderByIdAsync(Guid orderId);
    }
}
