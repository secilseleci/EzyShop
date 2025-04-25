using Core.Pagination;
using Core.Utilities.Results;
using Models.ViewModels.Auth;
using Models.ViewModels.Customer;

namespace Business.Services.Abstract;

public interface ICustomerService
{
    Task<IResult> CreateCustomerAsync(Guid userId, RegisterCustomerViewModel model);
    Task<IResult> DeleteCustomerAsync(Guid customerId);
    Task<IDataResult<PaginatedList<CustomerListViewModel>>> GetPaginatedCustomersAsync(
        int page,
        int pageSize,
        string? searchTerm = null);
    Task<int> CountAsync();
}