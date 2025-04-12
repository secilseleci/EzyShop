using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete;

public class CartRepository(ApplicationDbContext context) : BaseRepository<Cart>(context), ICartRepository
{
    public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
    {
        return await _dataContext.Carts
                .Include(c => c.CartLines)
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(c => c.Customer.UserId == userId && !c.IsDeleted);
    }
}
