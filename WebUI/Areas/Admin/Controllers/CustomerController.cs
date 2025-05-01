using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using WebUI.Controllers;

namespace WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]

public class CustomerController : BaseController
{
    private readonly ICustomerService _customerService;

    public CustomerController(
    ICustomerService customerService,
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    SignInManager<AppUser> signInManager,
    IWebHostEnvironment webHostEnvironment,
    IMapper mapper) : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _customerService = customerService;
    }
    #region List Customers
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    #endregion

    #region API
    [HttpGet]
    public async Task<IActionResult> GetPaginatedCustomers()
    {
        int start = int.Parse(Request.Query["start"]);
        int length = int.Parse(Request.Query["length"]);
        string? search = Request.Query["search[value]"];

        int page = (start / length) + 1;
        int pageSize = length;


        var result = await _customerService.GetPaginatedCustomerListAsync(search, page, length);

        if (!result.Success)
        {
            return Json(new { success = false, message = result.Message });
        }

        return Json(new
        {
            draw = int.Parse(Request.Query["draw"]),
            recordsTotal = result.Data.TotalItems,
            recordsFiltered = result.Data.TotalItems,
            data = result.Data.Items
        });
    }
    #endregion

    #region Delete Customer

    [HttpPost]
    public async Task<IActionResult> Delete(Guid customerId)
    {
        var result = await _customerService.DeleteCustomerAsync(customerId);
        if (result.Success)
        {
            return Json(new { success = true, message = result.Message });
        }
        return Json(new { success = false, message = result.Message });
    }

    #endregion
}
