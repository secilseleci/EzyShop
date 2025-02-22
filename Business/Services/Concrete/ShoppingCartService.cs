using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;


namespace Business.Services.Concrete
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;

        }

        public async Task<IDataResult<ShoppingCart>> GetCartByUserIdAsync(Guid userId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            return cart is not null
                ? new SuccessDataResult<ShoppingCart>(cart)
                : new ErrorDataResult<ShoppingCart>(Messages.ShoppingCartNotFound);
        }

        public async Task<ShoppingCart> GetOrCreateCartAsync(Guid userId)
        {
            var cartResult = await GetCartByUserIdAsync(userId);
            var cart = cartResult.Data;
            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    AppUserId = userId,
                    CartItems = new List<ShoppingCartItem>()
                };

                await _shoppingCartRepository.CreateAsync(cart);
            }
            return cart;
        }

       
    }
}
