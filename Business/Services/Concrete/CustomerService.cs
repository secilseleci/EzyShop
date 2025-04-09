using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Pagination;
using Core.Security;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels.Customer;
using System.Linq.Expressions;

namespace Business.Services.Concrete;

public class CustomerService : BaseService, ICustomerService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ICustomerRepository _customerRepo;
    public CustomerService(
      IMapper mapper,
      IConfiguration config,
      ICurrentUserService currentUser,
      UserManager<AppUser> userManager,
      RoleManager<AppRole> roleManager,
      ICustomerRepository customerRepo
  ) : base(mapper, config, currentUser)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _customerRepo = customerRepo;
    }

    public async Task<IResult> Register(RegisterViewModel model)
    {
        var user = Mapper.Map<AppUser>(model);
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return new ErrorResult(string.Join(" | ", result.Errors.Select(e => e.Description)));
        }

        await _userManager.AddToRoleAsync(user, "Customer");

        var customer = Mapper.Map<Customer>(model);
        customer.UserId = user.Id;
        customer.CreatedById = customer.UserId;

        await _customerRepo.CreateAsync(customer);

        return new SuccessResult(Messages.RegisterSuccess);
    }

    public async Task<IResult> DeleteCustomerAsync(Guid customerId)
    {
        if (!await _customerRepo.ExistsAsync(c => c.Id == customerId && !c.IsDeleted))
            return new ErrorResult(Messages.CustomerNotFound);

        var affectedRows = await _customerRepo.SoftDeleteAsync(customerId);

        return affectedRows > 0
            ? new SuccessResult(Messages.DeleteCustomerSuccess)
        : new ErrorResult(Messages.DeleteCustomerError);

    }



    public async Task<IDataResult<PaginatedList<CustomerListViewModel>>> GetPaginatedCustomersAsync(
    int page,
    int pageSize,
    string? searchTerm = null)
    {
        Expression<Func<Customer, bool>> predicate;

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            predicate = u =>
            u.User != null &&
         (
             (u.User.Name + " " + u.User.Surname).Contains(searchTerm) ||
             u.User.Email!.Contains(searchTerm)
         );

        }
        else
        {
            predicate = u => true;
        }

        var paginatedCustomers = await _customerRepo.GetPaginatedAsync(
            predicate,
            page,
            pageSize,
            q => q.Include(c => c.User));

        var viewModels = Mapper.Map<IEnumerable<CustomerListViewModel>>(paginatedCustomers.Items);

        var result = new PaginatedList<CustomerListViewModel>(
            viewModels,
            paginatedCustomers.TotalItems,
            paginatedCustomers.Page,
            paginatedCustomers.PageSize
        );

        return result.Items.Any()
            ? new SuccessDataResult<PaginatedList<CustomerListViewModel>>(result)
            : new ErrorDataResult<PaginatedList<CustomerListViewModel>>(Messages.EmptyCustomerList);
    }

}
