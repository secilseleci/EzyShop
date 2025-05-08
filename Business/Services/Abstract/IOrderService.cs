using Core.Utilities.Results;
using Models.Entities.Concrete;

namespace Business.Services.Abstract;

public interface IOrderService
{ 
    Task<Order?> GetInCartOrderAsync(Guid customerId);
    Task<IDataResult<Order>> GetOrCreateCartAndAddProductAsync(Guid productId, Guid customerId);
}
