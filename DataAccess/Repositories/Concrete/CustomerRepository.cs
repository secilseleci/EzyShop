using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete;

public class CustomerRepository(ApplicationDbContext context) : BaseRepository<Customer>(context), ICustomerRepository
{
    public async Task<Customer?> GetCustomerByUserIdAsync(Guid userId)
    {
        return await _dataContext.Customers
            .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted);
    }


    public async Task<IEnumerable<Customer>> GetCustomersWithOrdersAsync()
    {
        return await _dataContext.Customers
            .Include(c => c.Orders)
            .Where(c => !c.IsDeleted && c.Orders.Any())
            .ToListAsync();
    }


    public async Task<Customer?> GetCustomerWithCartAsync(Guid customerId)
    {
        return await _dataContext.Customers
            .Include(c => c.ShoppingCart)
            .FirstOrDefaultAsync(c => c.Id == customerId && !c.IsDeleted);
    }

}
