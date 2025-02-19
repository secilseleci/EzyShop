using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
 

namespace DataAccess.Repositories.Concrete
{
    public class ShoppingCartRepository(ApplicationDbContext context) : BaseRepository<ShoppingCart>(context), IShoppingCartRepository
    {
        public async Task<ShoppingCart?> GetCartByUserIdAsync(Guid userId)
        {
            return await _dataContext.ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.AppUserId == userId);
        }
    }
}
