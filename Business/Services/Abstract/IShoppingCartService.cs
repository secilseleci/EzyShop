using Core.Utilities.Results;
using Models.Entities.Concrete;
 

namespace Business.Services.Abstract;

public interface IShoppingCartService
{
    Task<ShoppingCart> GetOrCreateCartAsync(Guid userId);
    Task<IDataResult<ShoppingCart>> GetCartByCustomerIdAsync(Guid customerId);
     
}
