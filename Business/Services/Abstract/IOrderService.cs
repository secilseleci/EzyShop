using Core.Utilities.Results;
using Models.Entities.Concrete;

namespace Business.Services.Abstract;

public interface IOrderService
{
    Task<IDataResult<Order>> GetOrCreateCartAndAddProductAsync(Guid productId, Guid customerId);
}
