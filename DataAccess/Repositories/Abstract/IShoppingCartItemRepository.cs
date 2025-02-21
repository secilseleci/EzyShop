using Models.Entities.Concrete;
 
namespace DataAccess.Repositories.Abstract
{
    public interface IShoppingCartItemRepository : IBaseRepository<ShoppingCartItem>
    {
        Task<ShoppingCartItem?> GetCartItemAsync(Guid cartId, Guid productId);
        Task<int> RemoveCartItemAsync(Guid cartId, Guid productId);
        Task<int> ClearCartAndCheckDeleteAsync(Guid cartId);
        Task<int> RemoveItemFromCartAsync(Guid cartId, Guid productId);
        Task<int> RemoveMultipleItemsFromCartAsync(Guid cartId, IEnumerable<Guid> productIds);
        Task<int> IncreaseItemCountAsync(Guid cartId, Guid productId);
        Task<int> DecreaseItemCountAsync(Guid cartId, Guid productId);
        Task<int> GetTotalCartItemsAsync(Guid cartId);

    }
}
