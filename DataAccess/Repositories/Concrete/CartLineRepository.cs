using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;


namespace DataAccess.Repositories.Concrete
{
    public class CartLineRepository(ApplicationDbContext context) : BaseRepository<CartLine>(context), ICartLineRepository
    {


        #region Get Total Cart Lines
        public async Task<int> GetTotalCartLinesAsync(Guid cartId)
        {
            return await _dataContext.CartLines
                .Where(item => item.CartId == cartId && !item.IsDeleted)
                .SumAsync(item => item.Count);
        }

        #endregion
        #region Get Line By Cart and Product
        public async Task<CartLine?> GetLineByCartAndProductAsync(Guid cartId, Guid productId)
        {
            return await _dataContext.CartLines
                .FirstOrDefaultAsync(cl => cl.CartId == cartId && cl.ProductId == productId && !cl.IsDeleted);
        }
        #endregion
    }
}
