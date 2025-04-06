using Core.Utilities.Results;
using Models.ViewModels.Customer;

namespace Business.Services.Abstract;

public interface ICustomerService
{
    Task<IResult> Register(RegisterViewModel model);

}
