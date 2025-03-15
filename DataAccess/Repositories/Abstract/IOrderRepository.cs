using Models.Entities.Concrete;

namespace DataAccess.Repositories.Abstract
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order?> GetOrderWithCustomerAsync(Guid orderId);

        Task<List<Order>> GetOrdersWithDetailsAsync(Guid shopId);
        Task<Order?> GetOrderWithDetailsAsync(Guid orderId);
    }
}
