﻿using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels.Shop;

namespace Business.Services.Abstract
{
    public interface IShoppingCartItemService
    {
        Task<IResult> AddToCartAsync(Guid userId, Guid productId, int count);
        Task<IDataResult<ShoppingCartItem?>> GetCartItemByProductIdAsync(Guid cartId, Guid productId);
        Task<IDataResult<ShoppingCartItem?>> GetCartItemByIdAsync(Guid itemId);
        Task<IDataResult<IEnumerable<ShoppingCartItemViewModel>>> GetAllCartItemsAsync(Guid userId);
        
       
        Task<IResult> ClearCartAsync(Guid userId);
        Task<IResult> RemoveItemFromCartAsync(Guid userId, Guid productId);
        Task<IResult> RemoveMultipleItemsFromCartAsync(Guid userId, IEnumerable<Guid> productIds);
        Task<IResult> IncreaseItemCountAsync(Guid userId, Guid productId);
        Task<IResult> DecreaseItemCountAsync(Guid userId, Guid productId);

        Task<int>GetTotalCartItemsAsync(Guid userId);
 
    }
}
