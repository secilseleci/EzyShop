using Business.Services.Abstract;
using DataAccess.Repositories.Abstract;


namespace Business.Services.Concrete;

public class ShoppingCartService :IShoppingCartService
{
    private readonly IShoppingCartRepository _shoppingCartRepo;
    public ShoppingCartService(IShoppingCartRepository shoppingCartRepo)
    {
        _shoppingCartRepo = shoppingCartRepo;

    }

  

     

}
