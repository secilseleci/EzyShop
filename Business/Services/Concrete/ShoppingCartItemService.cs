using Business.Services.Abstract;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;


namespace Business.Services.Concrete
{
    public class ShoppingCartItemService : IShoppingCartItemService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartItemService(
            IShoppingCartRepository shoppingCartRepository,
            IShoppingCartItemRepository shoppingCartItemRepository,
            IShoppingCartService shoppingCartService)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _shoppingCartService = shoppingCartService;
        }
        #region AddItemToCart
        public async Task<IResult> AddItemToCartAsync(Guid userId, Guid productId, int count)
        {
            var cart = await _shoppingCartService.GetOrCreateCartAsync(userId);  

            var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Count += count;  
                await _shoppingCartItemRepository.UpdateAsync(existingItem);
            }
            else
            {
                var newItem = new ShoppingCartItem
                {
                    ProductId = productId,
                    CartId = cart.Id,
                    Count = count
                };
                await _shoppingCartItemRepository.CreateAsync(newItem);
            }

            return new SuccessResult("Product added to cart.");
        }
        #endregion
    }
}
}
