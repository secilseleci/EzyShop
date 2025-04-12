using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;

namespace Business.Services.Concrete;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepo;
    private readonly ICustomerRepository _customerRepo;

    public CartService(ICartRepository cartRepo, ICustomerRepository customerRepo)
    {
        _cartRepo = cartRepo;
        _customerRepo = customerRepo;
    }

    public async Task<IDataResult<Cart>> GetOrCreateCartAsync(Guid userId)
    {
        if (await _cartRepo.GetCartByUserIdAsync(userId) is Cart cart)
            return new SuccessDataResult<Cart>(cart);

        var customer = await _customerRepo.GetCustomerByUserIdAsync(userId);
        if (customer == null)
            return new ErrorDataResult<Cart>(Messages.CustomerNotFound);

        cart = new Cart
        {
            CustomerId = customer.Id
        }; 
        
        await _cartRepo.CreateAsync(cart);
        return new SuccessDataResult<Cart>(cart);

    }
}
