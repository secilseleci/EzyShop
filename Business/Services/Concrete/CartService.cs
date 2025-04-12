using Business.Services.Abstract;
using DataAccess.Repositories.Abstract;


namespace Business.Services.Concrete;

public class CartService :ICartService
{
    private readonly ICartRepository _cartRepo;
    public CartService(ICartRepository cartRepo)
    {
        _cartRepo = cartRepo;

    }

  

     

}
