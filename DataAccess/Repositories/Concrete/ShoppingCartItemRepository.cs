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

    }
}
