using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Admin;
using WebUI.Controllers;

namespace WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : BaseController
{
    private readonly IShopService _shopService;
    private readonly ICustomerService _customerService;

    public DashboardController
    (
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper,
        IShopService shopService,
        ICustomerService customerService) : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _shopService = shopService;
        _customerService = customerService;
    }
     
    public async Task<IActionResult> Index()
    {
        var model = new AdminDashboardViewModel
        {
            TotalCustomers = await _customerService.CountAsync(),
            PendingShops = await _shopService.CountPendingShopsAsync(),
            ActiveShops = await _shopService.CountActiveShopsAsync()
        };
        return View(model);
    }

}
