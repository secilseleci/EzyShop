using Core.Utilities.Results;
using Models.Entities.Concrete;

namespace Business.Services.Abstract
{
    public interface IShoppingCartItemService
    {
        Task<IResult> AddToCartAsync(Guid userId, Guid productId, int count);
        Task<IDataResult<IEnumerable<ShoppingCartItem>>> GetCartItemsAsync(Guid userId);

        Task<int>GetTotalCartItemsAsync(Guid userId);
        Task<IResult> RemoveItemFromCartAsync(Guid userId, Guid productId);

    }
}
