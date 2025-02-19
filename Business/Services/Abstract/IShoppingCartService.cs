using Core.Utilities.Results;
using Models.Entities.Concrete;
using System.Linq.Expressions;


namespace Business.Services.Abstract
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetOrCreateCartAsync(Guid userId);

    }
}
