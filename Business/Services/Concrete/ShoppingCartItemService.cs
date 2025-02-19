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

        #region 📌 AddItemToCart
        public async Task<IResult> AddToCartAsync(Guid userId, Guid productId, int count)
        {
            var cart = await _shoppingCartService.GetOrCreateCartAsync(userId);

            var existingItem = await _shoppingCartItemRepository.GetCartItemAsync(cart.Id, productId);

            if (existingItem != null)
            {
                existingItem.Count += count;
                await _shoppingCartItemRepository.UpdateAsync(existingItem);
            }
            else
            {
                var newItem = new ShoppingCartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Count = count
                };
                await _shoppingCartItemRepository.CreateAsync(newItem);
            }

            return new SuccessResult("Ürün sepete başarıyla eklendi.");
        }
        #endregion

        #region 🗑 RemoveItemFromCart
        public async Task<IResult> RemoveItemFromCartAsync(Guid userId, Guid productId)
        {
            var cart = await _shoppingCartService.GetOrCreateCartAsync(userId);

            var existingItem = await _shoppingCartItemRepository.GetCartItemAsync(cart.Id, productId);

            if (existingItem == null)
            {
                return new ErrorResult("Sepette böyle bir ürün bulunamadı.");
            }

            var deleteResult = await _shoppingCartItemRepository.DeleteAsync(existingItem.Id);
            return deleteResult > 0
                ? new SuccessResult("Ürün sepetten kaldırıldı.")
                : new ErrorResult("Ürün kaldırılırken bir hata oluştu.");
        }
        #endregion

        #region 🛒 GetCartItems
        public async Task<IDataResult<IEnumerable<ShoppingCartItem>>> GetCartItemsAsync(Guid userId)
        {
            var cart = await _shoppingCartService.GetOrCreateCartAsync(userId);
            var cartItems = await _shoppingCartItemRepository.GetAllAsync(i => i.CartId == cart.Id);

            return cartItems is not null && cartItems.Any()
                ? new SuccessDataResult<IEnumerable<ShoppingCartItem>>(cartItems)
                : new ErrorDataResult<IEnumerable<ShoppingCartItem>>("Sepetinizde ürün bulunmamaktadır.");
        }
        #endregion

        #region 🔢 GetTotalCartItems
        public async Task<int> GetTotalCartItemsAsync(Guid userId)
        {
            var cart = await _shoppingCartService.GetOrCreateCartAsync(userId);
            return await _shoppingCartItemRepository.GetTotalCartItemsAsync(cart.Id);
        }
        #endregion
    }
}
