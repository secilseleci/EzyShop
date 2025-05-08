using Models.DTOs.OrderItem;

namespace Business.Services.Abstract;

public interface IOrderItemService
{
    Task<IEnumerable<OrderItemDto>> GetOrderItemsAsync(Guid orderId);

}
