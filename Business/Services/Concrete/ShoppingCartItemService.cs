using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using DataAccess.Repositories.Concrete;
using Models.Entities.Concrete;
using Models.ViewModels;


namespace Business.Services.Concrete
{
    public class ShoppingCartItemService : IShoppingCartItemService
    {
        private readonly IProductRepository _productRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IMapper _mapper;

        public ShoppingCartItemService(
            IProductRepository productRepository,
            IShoppingCartRepository shoppingCartRepository,
            IShoppingCartItemRepository shoppingCartItemRepository,
            IShoppingCartService shoppingCartService,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _shoppingCartService = shoppingCartService;
            _mapper = mapper;
        }

        #region 📌 Add Item 
        public async Task<IResult> AddToCartAsync(Guid userId, Guid productId, int count)
        {
            if (count < 1 || count > 100)
            {
                return new ErrorResult(Messages.CartItemCountError);
            }
            var cart = await _shoppingCartService.GetOrCreateCartAsync(userId);
            var cartItemResult = await GetCartItemAsync(cart.Id, productId);
           

            if (cartItemResult.Success)
            {
                var existingItem = cartItemResult.Data;
                if (existingItem.Count + count > 100)
                {
                    return new ErrorResult(Messages.CartItemCountError);
                }

                existingItem.Count += count;
                await _shoppingCartItemRepository.UpdateAsync(existingItem);
            }
            else
            {
                if (count > 100)
                {
                    return new ErrorResult(Messages.CartItemCountError);
                }
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                    return new ErrorResult(Messages.ShoppingCartItemNotFound);
                var newItem = new ShoppingCartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Count = count,
                    Price = product.Price
                };
                await _shoppingCartItemRepository.CreateAsync(newItem);
            }

            return new SuccessResult(Messages.AddShoppingCartItemSuccess);
        }
        #endregion 

        #region Get/List Item
        public async Task<IDataResult<ShoppingCartItem?>> GetCartItemAsync(Guid cartId, Guid productId)
        {
            var cartItem = await _shoppingCartItemRepository.GetCartItemAsync(cartId, productId);
            return cartItem is not null
                ? new SuccessDataResult<ShoppingCartItem?>(cartItem)
                : new ErrorDataResult<ShoppingCartItem?>(Messages.ShoppingCartItemNotFound);
        }

        public async Task<IDataResult<IEnumerable<ShoppingCartItemViewModel>>> GetAllCartItemsAsync(Guid userId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                return new ErrorDataResult<IEnumerable<ShoppingCartItemViewModel>>(Messages.ShoppingCartNotFound);

            var cartItems = await _shoppingCartItemRepository.GetCartItemsAsync(cart.Id);
            if (!cartItems.Any())  
            {
                return new SuccessDataResult<IEnumerable<ShoppingCartItemViewModel>>(new List<ShoppingCartItemViewModel>(), Messages.ShoppingCartEmpty);
            }
            var cartItemViewModels = _mapper.Map<IEnumerable<ShoppingCartItemViewModel>>(cartItems);

            return new SuccessDataResult<IEnumerable<ShoppingCartItemViewModel>>(cartItemViewModels);
        }
        #endregion

        #region 🗑 Remove 

        public async Task<IResult> ClearCartAsync(Guid userId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return new SuccessResult(Messages.ShoppingCartIsAlreadyEmpty);
            }

            var deleteResult = await _shoppingCartItemRepository.ClearCartAndCheckDeleteAsync(cart.Id);
            return deleteResult > 0
                ? new SuccessResult(Messages.ClearCartSuccess)
                : new ErrorResult(Messages.ClearCartError);
        }

        public async Task<IResult> RemoveItemFromCartAsync(Guid userId, Guid productId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return new ErrorResult(Messages.ShoppingCartNotFound);
            }
            var cartItem = await _shoppingCartItemRepository.GetCartItemAsync(cart.Id, productId);
            if (cartItem == null)
            {
                return new ErrorResult(Messages.ProductIsNotInYourCart);
            }
            var deleteResult = await _shoppingCartItemRepository.RemoveCartItemAsync(cart.Id, productId);

            return deleteResult > 0
                ? new SuccessResult(Messages.DeleteShoppingCartItemSuccess)
                : new ErrorResult(Messages.DeleteShoppingCartItemError);
        }
        public async Task<IResult> RemoveMultipleItemsFromCartAsync(Guid userId, IEnumerable<Guid> productIds)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return new ErrorResult(Messages.ShoppingCartNotFound);
            }

            var deleteResult = await _shoppingCartItemRepository.RemoveMultipleItemsFromCartAsync(cart.Id, productIds);

            return deleteResult > 0
                ? new SuccessResult(Messages.RemoveMultipleItemsFromCartSuccess)
                : new ErrorResult(Messages.RemoveMultipleItemsFromCartError);
        }

        #endregion
 
        #region Change Count
        public async Task<IResult> IncreaseItemCountAsync(Guid userId, Guid productId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return new ErrorResult(Messages.ShoppingCartNotFound);

            var cartItem = await _shoppingCartItemRepository.GetCartItemAsync(cart.Id, productId);
            if (cartItem == null) return new ErrorResult(Messages.ProductIsNotInYourCart);
            if (cartItem.Count >= 100)
            {
                return new ErrorResult(Messages.CartItemCountError);
            }
            var updateResult = await _shoppingCartItemRepository.IncreaseItemCountAsync(cart.Id, productId);
            return updateResult > 0
                 ? new SuccessResult(Messages.ChangeCountSuccess)
                : new ErrorResult(Messages.ChangeCountError);
        }

        public async Task<IResult> DecreaseItemCountAsync(Guid userId, Guid productId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return new ErrorResult(Messages.ShoppingCartNotFound);

            var cartItem = await _shoppingCartItemRepository.GetCartItemAsync(cart.Id, productId);
            if (cartItem == null) return new ErrorResult(Messages.ProductIsNotInYourCart);
            if (cartItem.Count <= 1)
            {
                return new ErrorResult(Messages.CartItemCountError);
            }
            var updateResult = await _shoppingCartItemRepository.DecreaseItemCountAsync(cart.Id, productId);
            return updateResult > 0
                ? new SuccessResult(Messages.ChangeCountSuccess)
                : new ErrorResult(Messages.ChangeCountError);
        }

        #endregion

        #region 🔢 Total CartItems
        public async Task<int> GetTotalCartItemsAsync(Guid userId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return 0;  
            }

            return await _shoppingCartItemRepository.GetTotalCartItemsAsync(cart.Id);
        }


        #endregion
    }
}
