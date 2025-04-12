using Core.Utilities.Results;
using Models.ViewModels.Product;

namespace Business.Services.Abstract;

public interface ICartLineService
{
    
    Task<int>GetTotalCartLinesAsync(Guid userId);
    Task<IResult> CreateCartLineAsync(Guid userId,Guid productId);

}
