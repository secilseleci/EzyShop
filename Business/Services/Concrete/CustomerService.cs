using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Security;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels.Customer;

namespace Business.Services.Concrete;

public class CustomerService : BaseService,ICustomerService
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
        customer.CreatedById = customer.UserId ;

        await _customerRepo.CreateAsync(customer);
 
        return new SuccessResult(Messages.RegisterSuccess);
    }

}
