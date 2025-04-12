using AutoMapper;
using Business.Services.Abstract;
using DataAccess.Repositories.Abstract;

namespace Business.Services.Concrete;

public class ShoppingCartItemService : IShoppingCartItemService
{
    private readonly IProductRepository _productRepo;
    private readonly IShoppingCartRepository _shoppingCartRepo;
    private readonly IShoppingCartItemRepository _shoppingCartItemRepo;
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IMapper _mapper;

    public ShoppingCartItemService(
        IProductRepository productRepo,
        IShoppingCartRepository shoppingCartRepo,
        IShoppingCartItemRepository shoppingCartItemRepo,
        IShoppingCartService shoppingCartService,
        IMapper mapper)
    {
        _productRepo = productRepo;
        _shoppingCartRepo = shoppingCartRepo;
        _shoppingCartItemRepo = shoppingCartItemRepo;
        _shoppingCartService = shoppingCartService;
        _mapper = mapper;
    }
    #region GetTotalCartItems
    public async Task<int> GetTotalCartItemsAsync(Guid userId)
    {
        var cart = await _shoppingCartRepo.GetCartByUserIdAsync(userId);

        if (cart == null)
            return 0;

        return await _shoppingCartItemRepo.GetTotalCartItemsAsync(cart.Id);
    }

    #endregion


}
