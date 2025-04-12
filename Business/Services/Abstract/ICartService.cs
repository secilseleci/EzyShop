using Core.Utilities.Results;
using Models.Entities.Concrete;

namespace Business.Services.Abstract;

public interface ICartService
{
    Task<IDataResult<Cart>> GetOrCreateCartAsync(Guid userId);

}
