namespace Business.Services.Abstract;

public interface IShoppingCartItemService
{
    
    Task<int>GetTotalCartItemsAsync(Guid userId);

}
