using Models.Entities.Concrete;
 
namespace DataAccess.Repositories.Abstract;

public interface ICartRepository : IBaseRepository<Cart>
{
    Task<Cart?> GetCartByUserIdAsync(Guid userId);

}
