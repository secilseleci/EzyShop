using Models.Entities.Concrete;

namespace DataAccess.Repositories.Abstract;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<int> CountAsync();
}
