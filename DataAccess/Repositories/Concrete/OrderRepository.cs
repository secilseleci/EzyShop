using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete
{
    public class OrderRepository(ApplicationDbContext context) : BaseRepository<Order>(context), IOrderRepository
    {
        public async Task<List<Order>> GetOrdersWithDetailsAsync(Guid shopId)
        {
            return await _dataContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .Where(o => o.ShopId == shopId)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithDetailsAsync(Guid orderId)
        {
            var order = await _dataContext.Orders
                .Include(c => c.OrderItems)
                .Include(o => o.Customer)
                .Include(o => o.Shop)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            return order;
        }
    }
}
