using Models.Entities.Concrete;

namespace DataAccess.Repositories.Abstract;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<Order?> GetOrderWithCustomerAsync(Guid orderId);
    Task<Order?> GetOrderWithDetailsAsync(Guid orderId);

    Task<IEnumerable<Order>> GetAllOrdersWithDetailsAsync(Guid shopId);
}
