using AutoMapper;
using Core.Security;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Concrete;
using Models.Identity;
using System.Linq.Expressions;
using WebUI.Controllers;

namespace WebUI.Areas.Admin.Controllers;
[Area("Admin")]
[Route("Admin/[controller]/[action]")]
[Authorize(Roles = "Admin")]
public class UsersController : BaseController
{
    private readonly ICustomerRepository _customerRepo;
    public UsersController
    (
    ICustomerRepository customerRepo,
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    SignInManager<AppUser> signInManager,
    IWebHostEnvironment webHostEnvironment,
    IMapper mapper)
    : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _customerRepo = customerRepo;
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
    public async Task<IActionResult> GetPaginatedUsers(int page = 1, int pageSize = 10)
    {
        Expression<Func<Customer, bool>> predicate = u => true;

        var result = await _customerRepo.GetPaginatedAsync(predicate, page, pageSize);

        var data = result.Items.Select(c => new
        {
            Id = c.User.Id!,
            Name = c.User.FullName ?? "N/A",
            Email = c.User.Email ?? "N/A",
            PhoneNumber = c.User.PhoneNumber ?? "-"
        });

        return Json(new { data });

    }
    #endregion

    #region Delete Customer

    [HttpPost]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var user = await UserManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Json(new { success = false, message = "Kullanıcı bulunamadı." });
        }

        var result = await UserManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return Json(new { success = true, message = "Kullanıcı başarıyla silindi." });
        }
        return Json(new { success = false, message = "Kullanıcı silinemedi." });
    }

    #endregion


}
