using AutoMapper;
using Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using WebUI.Controllers;

namespace WebUI.Areas.Admin.Controllers;
[Area("Admin")]
[Route("Admin/[controller]/[action]")]
[Authorize(Roles = "Admin")]
public class SellerController : BaseController
{
   

    public SellerController
    (ICurrentUserService currentUserService,
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    SignInManager<AppUser> signInManager,
    IWebHostEnvironment webHostEnvironment,
    IMapper mapper)
    : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
    }
    public IActionResult Index()
    {
        return View();
    }

    //#region SellerApplication

    //[HttpGet]
    //public async Task<IActionResult> ListSellerApplications()
    //{
    //    var result = await _sellerApplicationService.GetAllSellerApplicationsAsync(a => true);

    //    if (!result.Success)
    //    {
    //        TempData["ErrorMessage"] = result.Message;
    //        return RedirectToAction("Index", "Home");
    //    }

    //    return View(result.Data);
    //}

    //[HttpGet]
    //public async Task<IActionResult> GetSellerApplications(string status = "all")
    //{
    //    Expression<Func<SellerApplication, bool>> filter = status.ToLower() switch
    //    {
    //        "approved" => a => a.Status == ApplicationStatus.Approved,
    //        "rejected" => a => a.Status == ApplicationStatus.Rejected,
    //        "pending" => a => a.Status == ApplicationStatus.Pending,
    //        _ => a => true
    //    };

    //    var result = await _sellerApplicationService.GetAllSellerApplicationsAsync(filter);

    //    if (!result.Success || result.Data == null || !result.Data.Any())
    //    {
    //        return PartialView("_SellerApplicationsTable", new List<SellerApplication>()); // Boş liste döndür
    //    }

    //    return PartialView("_SellerApplicationsTable", result.Data);
    //}

    //[HttpPost]
    //public async Task<IActionResult> ApproveSeller(Guid id)
    //{
    //    var result = await _sellerApplicationService.ApproveSellerAsync(id);
    //    if (result.Success)
    //    {
    //        return Json(new { success = true, message = result.Message });
    //    }
    //    else
    //    {
    //        return Json(new { success = false, message = result.Message });
    //    }
    //}

    //[HttpPost]
    //public async Task<IActionResult> RejectSeller(Guid id)
    //{
    //    var result = await _sellerApplicationService.RejectSellerAsync(id);
    //    TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
    //    return RedirectToAction("ListSellerApplications");
    //}
    //#endregion
}
