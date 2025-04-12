using AutoMapper;
using Business.Services.Abstract;
using DataAccess.Repositories.Abstract;

namespace Business.Services.Concrete;

public class CartLineService : ICartLineService
{
    private readonly IProductRepository _productRepo;
    private readonly ICartRepository _cartRepo;
    private readonly ICartLineRepository _cartLineRepo;
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;

    public CartLineService(
        IProductRepository productRepo,
        ICartRepository cartRepo,
        ICartLineRepository cartLineRepo,
        ICartService cartService,
        IMapper mapper)
    {
        _productRepo = productRepo;
        _cartRepo = cartRepo;
        _cartLineRepo = cartLineRepo;
        _cartService = cartService;
        _mapper = mapper;
    }
    #region GetTotalCartLines
    public async Task<int> GetTotalCartLinesAsync(Guid userId)
    {
        var cart = await _cartRepo.GetCartByUserIdAsync(userId);

        if (cart == null)
            return 0;

        return await _cartLineRepo.GetTotalCartLinesAsync(cart.Id);
    }

    #endregion


}
