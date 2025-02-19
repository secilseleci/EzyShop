using Models.Entities.Concrete;
 
namespace DataAccess.Repositories.Abstract
{
    public interface IShoppingCartItemRepository : IBaseRepository<ShoppingCartItem>
    {
        Task<ShoppingCartItem?> GetCartItemAsync(Guid cartId, Guid productId);
        Task<int> GetTotalCartItemsAsync(Guid userId);

    }
}
