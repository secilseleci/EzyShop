using AutoMapper;
using Business.Services.Abstract;
using Core.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Customer;

namespace WebUI.Controllers;

public class CustomerController : BaseController
{
    private readonly ICustomerService _customerService;
    public CustomerController(
        ICurrentUserService currentUserService, 
        ICustomerService customerService,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
         : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _customerService = customerService;
    }

    #region Register
    public IActionResult Register()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var result = await _customerService.Register(model);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View("Register", model);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction("Login", "Account", routeValues: new { email = model.Email, returnUrl = string.Empty });
    }
    #endregion

}
