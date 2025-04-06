using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete;

public class OrderItemRepository(ApplicationDbContext context) :BaseRepository<OrderItem>(context), IOrderItemRepository
{
   
}
