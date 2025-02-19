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
        public async Task<IResult> AddToCartAsync(Guid userId, Guid productId, int quantity)
        {
            var cart = await _shoppingCartService.GetOrCreateCartAsync(userId);

            var existingItem = await _shoppingCartItemRepository.GetCartItemAsync(cart.Id, productId);

            if (existingItem != null)
            {
                existingItem.Count += quantity;
                await _shoppingCartItemRepository.UpdateAsync(existingItem);
            }
            else
            {
                var newItem = new ShoppingCartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Count = quantity
                };
                await _shoppingCartItemRepository.CreateAsync(newItem);
            }

            return new SuccessResult("Ürün sepete başarıyla eklendi.");
        }
        #endregion
    }
}
 
