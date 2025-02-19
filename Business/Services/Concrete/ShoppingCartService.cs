using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;
using System.Linq.Expressions;


namespace Business.Services.Concrete
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IMapper _mapper;
        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository,IMapper mapper)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _mapper = mapper;

        }
 
        
        

        public async Task<ShoppingCart> GetOrCreateCartAsync(Guid userId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new ShoppingCart { AppUserId = userId };
                await _shoppingCartRepository.CreateAsync(cart);
            }
            return cart;
        }

       
    }
}
