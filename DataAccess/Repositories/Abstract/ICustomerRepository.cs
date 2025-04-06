using Models.Entities.Concrete;

namespace DataAccess.Repositories.Abstract;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<Customer?> GetCustomerWithCartAsync(Guid customerId);
    Task<Customer?> GetCustomerByUserIdAsync(Guid userId);
    Task<IEnumerable<Customer>> GetCustomersWithOrdersAsync();


}
