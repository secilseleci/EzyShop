using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels;
using System.Linq.Expressions;

namespace Business.Services.Abstract
{
    public interface IShopService
    {
        Task<IDataResult<ShopViewModel>> GetShopBySellerIdAsync(Guid sellerId);
        Task<IResult> CreateShopAsync(Shop entity);
        Task<IDataResult<Shop>> GetShopByIdAsync(Guid shopId);
        Task<IDataResult<IEnumerable<Shop>>> GetAllShopsAsync(Expression<Func<Shop, bool>> predicate);


        Task<IResult> UpdateShopAsync(ShopViewModel model);
        Task<IResult> DeleteShopAsync(Guid shopId);
    }
}
