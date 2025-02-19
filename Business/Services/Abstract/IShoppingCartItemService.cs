using Core.Utilities.Results;

namespace Business.Services.Abstract
{
    public interface IShoppingCartItemService
    {
        Task<IResult> AddToCartAsync(Guid userId, Guid productId, int quantity);


    }
}
