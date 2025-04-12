using Models.Entities.Concrete;
 
namespace DataAccess.Repositories.Abstract;

public interface ICartLineRepository : IBaseRepository<CartLine>
{
  
    Task<int> GetTotalCartLinesAsync(Guid cartId);



}
