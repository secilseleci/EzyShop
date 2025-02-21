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

        #region 📌 Add Item 
        public async Task<IResult> AddToCartAsync(Guid userId, Guid productId, int count)
        {
            var cart = await _shoppingCartService.GetOrCreateCartAsync(userId);
            var cartItemResult = await GetCartItemAsync(cart.Id, productId);  

            if (cartItemResult.Success)
            {
                cartItemResult.Data.Count += count;
                await _shoppingCartItemRepository.UpdateAsync(cartItemResult.Data);
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

        #region 🗑 Remove 

        public async Task<IResult> ClearCartAsync(Guid userId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return new ErrorResult("Sepetiniz zaten boş.");
            }

            var deleteResult = await _shoppingCartItemRepository.ClearCartAndCheckDeleteAsync(cart.Id);
            return deleteResult > 0
                ? new SuccessResult("Sepetiniz başarıyla temizlendi.")
                : new ErrorResult("Sepet temizleme işlemi başarısız.");
        }

        public async Task<IResult> RemoveItemFromCartAsync(Guid userId, Guid productId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return new ErrorResult("Sepetiniz bulunmamaktadır.");
            }

            var deleteResult = await _shoppingCartItemRepository.RemoveItemFromCartAsync(cart.Id, productId);

            return deleteResult > 0
                ? new SuccessResult("Ürün sepetinizden başarıyla kaldırıldı.")
                : new ErrorResult("Ürün kaldırma işlemi başarısız.");
        }
        public async Task<IResult> RemoveMultipleItemsFromCartAsync(Guid userId, IEnumerable<Guid> productIds)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return new ErrorResult("Sepetiniz bulunmamaktadır.");
            }

            var deleteResult = await _shoppingCartItemRepository.RemoveMultipleItemsFromCartAsync(cart.Id, productIds);

            return deleteResult > 0
                ? new SuccessResult("Seçili ürünler sepetinizden başarıyla kaldırıldı.")
                : new ErrorResult("Ürünleri kaldırma işlemi başarısız.");
        }

        #endregion

        #region Get Item
        public async Task<IDataResult<ShoppingCartItem?>> GetCartItemAsync(Guid cartId, Guid productId)
        {
            var cartItem = await _shoppingCartItemRepository.GetCartItemAsync(cartId, productId);
            return cartItem is not null
                ? new SuccessDataResult<ShoppingCartItem?>(cartItem)
                : new ErrorDataResult<ShoppingCartItem?>("Bu ürün sepetinizde bulunmamaktadır.");
        }
        #endregion

        #region 🛒 List Items
        public async Task<IDataResult<IEnumerable<ShoppingCartItem>>> GetAllCartItemsAsync(Guid userId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return new ErrorDataResult<IEnumerable<ShoppingCartItem>>("Sepetinizde ürün bulunmamaktadır.");
            }

            var cartItems = await _shoppingCartItemRepository.GetAllAsync(i => i.CartId == cart.Id);

            return cartItems is not null && cartItems.Any()
                ? new SuccessDataResult<IEnumerable<ShoppingCartItem>>(cartItems)
                : new ErrorDataResult<IEnumerable<ShoppingCartItem>>("Sepetinizde ürün bulunmamaktadır.");
        }
        #endregion

        #region Change Count
        public async Task<IResult> IncreaseItemCountAsync(Guid userId, Guid productId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return new ErrorResult("Sepetiniz bulunmamaktadır.");

            var cartItem = await _shoppingCartItemRepository.GetCartItemAsync(cart.Id, productId);
            if (cartItem == null) return new ErrorResult("Bu ürün sepetinizde bulunmamaktadır.");

            var updateResult = await _shoppingCartItemRepository.IncreaseItemCountAsync(cart.Id, productId);
            return updateResult > 0
                ? new SuccessResult("Ürün adedi artırıldı.")
                : new ErrorResult("Ürün adedi artırılırken hata oluştu.");
        }

        public async Task<IResult> DecreaseItemCountAsync(Guid userId, Guid productId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return new ErrorResult("Sepetiniz bulunmamaktadır.");

            var cartItem = await _shoppingCartItemRepository.GetCartItemAsync(cart.Id, productId);
            if (cartItem == null) return new ErrorResult("Bu ürün sepetinizde bulunmamaktadır.");

            var updateResult = await _shoppingCartItemRepository.DecreaseItemCountAsync(cart.Id, productId);
            return updateResult > 0
                ? new SuccessResult("Ürün adedi azaltıldı.")
                : new ErrorResult("Ürün adedi azaltılırken hata oluştu.");
        }

        #endregion

        #region 🔢 Total CartItems
        public async Task<IDataResult<int>> GetTotalCartItemsAsync(Guid userId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return new ErrorDataResult<int>(0, "Sepetiniz bulunmamaktadır.");

            var totalItems = await _shoppingCartItemRepository.GetTotalCartItemsAsync(cart.Id);
            return new SuccessDataResult<int>(totalItems);
        }

        #endregion
    }
}
