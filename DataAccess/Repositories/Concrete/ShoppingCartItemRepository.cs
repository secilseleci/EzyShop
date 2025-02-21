using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
 

namespace DataAccess.Repositories.Concrete
{
    public class ShoppingCartItemRepository(ApplicationDbContext context) : BaseRepository<ShoppingCartItem>(context), IShoppingCartItemRepository
    {
        public async Task<ShoppingCartItem?> GetCartItemAsync(Guid cartId, Guid productId)
        {
            return await _dataContext.ShoppingCartItems
                .FirstOrDefaultAsync(i => i.CartId == cartId && i.ProductId == productId);
        }
        public async Task<int> RemoveCartItemAsync(Guid cartId, Guid productId)
        {
            var cartItem = await _dataContext.ShoppingCartItems
                            .FirstOrDefaultAsync(i => i.CartId == cartId && i.ProductId == productId);

            if (cartItem != null)
            {
                _dataContext.ShoppingCartItems.Remove(cartItem);
                return await _dataContext.SaveChangesAsync();
            }
            return 0;
        }
        public async Task<int> ClearCartAndCheckDeleteAsync(Guid cartId)
        {
            var cartItems = await _dataContext.ShoppingCartItems
                                .Where(i => i.CartId == cartId)
                                .ToListAsync();

            if (cartItems.Any())
            {
                _dataContext.ShoppingCartItems.RemoveRange(cartItems);
                await _dataContext.SaveChangesAsync();
            }

            var cart = await _dataContext.ShoppingCarts.FindAsync(cartId);
            if (cart != null)
            {
                _dataContext.ShoppingCarts.Remove(cart);
            }

            return await _dataContext.SaveChangesAsync();
        }
        public async Task<int> RemoveItemFromCartAsync(Guid cartId, Guid productId)
        {
            var cartItem = await _dataContext.ShoppingCartItems
                                .FirstOrDefaultAsync(i => i.CartId == cartId && i.ProductId == productId);

            if (cartItem == null)
            {
                return 0; 
            }

            _dataContext.ShoppingCartItems.Remove(cartItem);
            return await _dataContext.SaveChangesAsync();
        }
        public async Task<int> RemoveMultipleItemsFromCartAsync(Guid cartId, IEnumerable<Guid> productIds)
        {
            var cartItems = await _dataContext.ShoppingCartItems
                                .Where(i => i.CartId == cartId && productIds.Contains(i.ProductId))
                                .ToListAsync();

            if (!cartItems.Any())
            {
                return 0;  
            }

            _dataContext.ShoppingCartItems.RemoveRange(cartItems);
            return await _dataContext.SaveChangesAsync();
        }
        public async Task<int> IncreaseItemCountAsync(Guid cartId, Guid productId)
        {
            var cartItem = await _dataContext.ShoppingCartItems
                                .FirstOrDefaultAsync(i => i.CartId == cartId && i.ProductId == productId);

            if (cartItem == null || cartItem.Count >= 100)
            {
                return 0; 
            }

            cartItem.Count += 1;
            return await _dataContext.SaveChangesAsync();
        }

        public async Task<int> DecreaseItemCountAsync(Guid cartId, Guid productId)
        {
            var cartItem = await _dataContext.ShoppingCartItems
                                .FirstOrDefaultAsync(i => i.CartId == cartId && i.ProductId == productId);

            if (cartItem == null || cartItem.Count <= 1)
            {
                return 0; 
            }

            cartItem.Count -= 1;
            return await _dataContext.SaveChangesAsync();
        }
        public async Task<int> GetTotalCartItemsAsync(Guid cartId)
        {
            return await _dataContext.ShoppingCartItems
                .Where(i => i.CartId == cartId)
                .SumAsync(i => i.Count);
        }


    }
}
