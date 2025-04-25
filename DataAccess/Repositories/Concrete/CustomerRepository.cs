using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;


namespace DataAccess.Repositories.Concrete;

public class CustomerRepository(ApplicationDbContext context) : BaseRepository<Customer>(context), ICustomerRepository
{
    public async Task<int> CountAsync()
    {
        return await _dataContext.Customers
            .Where(c => !c.IsDeleted)
            .CountAsync();
    }
}