using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;

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

    #region Create CartLine

    public async Task<IResult> CreateCartLineAsync(Guid userId, Guid productId)
    {
        if (userId == Guid.Empty)
            return new ErrorResult(Messages.UserNotAuthenticated);
        
        using var transaction = await _cartRepo.BeginTransactionAsync();
        try
        {
            var cartResult = await _cartService.GetOrCreateCartAsync(userId);
            if (!cartResult.Success || cartResult.Data == null)
                return new ErrorResult(Messages.CartNotFound);

            var cart = cartResult.Data;

            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null || product.IsDeleted || !product.IsActive)
                return new ErrorResult(Messages.ProductNotFound);

            var existingLine = await _cartLineRepo.GetLineByCartAndProductAsync(cart.Id, productId);

            if (existingLine != null)
            {
                if (existingLine.Count >= product.Stock)
                { 
                    await transaction.RollbackAsync();
                    return new ErrorResult(Messages.InsufficientStock);
                }

                existingLine.Count += 1;
                await _cartLineRepo.UpdateAsync(existingLine);
            }

            else
            {
                var newLine = new CartLine
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Count = 1
                };
                await _cartLineRepo.CreateAsync(newLine);
                 
            }
            await transaction.CommitAsync();
            return new SuccessResult(Messages.ProductAddedSuccess);
        }
        catch(Exception)
        {
            await transaction.RollbackAsync();
            return new ErrorResult(Messages.ProductAddedError);

        }
    }

    #endregion
}
