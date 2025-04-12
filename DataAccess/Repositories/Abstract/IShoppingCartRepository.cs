using Models.Entities.Concrete;
 
namespace DataAccess.Repositories.Abstract;

public interface IShoppingCartRepository : IBaseRepository<ShoppingCart>
{
    Task<ShoppingCart?> GetCartByUserIdAsync(Guid userId);

}
