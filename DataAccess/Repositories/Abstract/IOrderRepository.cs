using Models.Entities.Concrete;

namespace DataAccess.Repositories.Abstract
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<List<Order>> GetOrdersWithDetailsAsync(Guid shopId);
    }
}
