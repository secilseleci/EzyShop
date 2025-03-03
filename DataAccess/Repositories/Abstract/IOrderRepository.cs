using Models.Entities.Concrete;

namespace DataAccess.Repositories.Abstract
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<int> CancelOrdersByEntityAsync(Guid entityId, string entityType);

    }
}
