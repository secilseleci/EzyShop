using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;


namespace Business.Services.Concrete;

public class ShoppingCartService :IShoppingCartService
{
    private readonly IShoppingCartRepository _shoppingCartRepo;
    public ShoppingCartService(IShoppingCartRepository shoppingCartRepo)
    {
        _shoppingCartRepo = shoppingCartRepo;

    }

    public async Task<IDataResult<ShoppingCart>> GetCartByCustomerIdAsync(Guid customerId)
    {
        var cart = await _shoppingCartRepo.GetCartByCustomerIdAsync(customerId);
        return cart is not null
            ? new SuccessDataResult<ShoppingCart>(cart)
            : new ErrorDataResult<ShoppingCart>(Messages.ShoppingCartNotFound);
    }

    public async Task<ShoppingCart> GetOrCreateCartAsync(Guid customerId)
    {
        var cartResult = await GetCartByCustomerIdAsync(customerId);
        var cart = cartResult.Data;
        if (cart == null)
        {
            cart = new ShoppingCart
            {
                CustomerId = customerId,
                CartItems = new List<ShoppingCartItem>()
            };

            await _shoppingCartRepo.CreateAsync(cart);
        }
        return cart;
    }


}
