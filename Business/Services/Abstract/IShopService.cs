using Core.Utilities.Results;
using Models.Entities.Concrete;
using Models.ViewModels;

namespace Business.Services.Abstract
{
    public interface IShopService
    {
        Task<IDataResult<ShopViewModel>> GetShopBySellerIdAsync(Guid sellerId);
        Task<IResult> CreateShopAsync(Shop entity);
     
    }
}
