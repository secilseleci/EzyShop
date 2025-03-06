using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels.Shop;
using System.Linq.Expressions;

namespace Business.Services.Abstract
{
    public interface IShopService
    {
        Task<IResult> CreateShopAsync(Shop entity);
        Task<IResult> DeleteShopAsync(Guid shopId);
        Task<IResult> UpdateShopAsync(ShopViewModel model);
        Task<IDataResult<Shop>> GetShopByIdAsync(Guid shopId);
        Task<IDataResult<IEnumerable<Shop>>> GetAllShopsAsync(Expression<Func<Shop, bool>> predicate);
        Task<IDataResult<ShopViewModel>> GetShopBySellerIdAsync(Guid sellerId);


    }
}
