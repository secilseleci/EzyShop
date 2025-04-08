using AutoMapper;
using Business.Services.Abstract;
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

    private readonly ISellerService _sellerService;
    public SellerController
    (ISellerService sellerService,
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    SignInManager<AppUser> signInManager,
    IWebHostEnvironment webHostEnvironment,
    IMapper mapper)
    : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _sellerService = sellerService;
    }
  
    #region List Sellers
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    #endregion

    #region API
    [HttpGet]
    public async Task<IActionResult> GetPaginatedSellers()
    {
        int start = int.Parse(Request.Query["start"]);
        int length = int.Parse(Request.Query["length"]);
        string? search = Request.Query["search[value]"];

        int page = (start / length) + 1;
        int pageSize = length;


        var result = await _sellerService.GetPaginatedSellersAsync(page, length, search);

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

    #region Delete Seller

    [HttpPost]
    public async Task<IActionResult> Delete(Guid sellerId)
    {

        var result = await _sellerService.DeleteSellerAsync(sellerId);
        if (result.Success)
        {
            return Json(new { success = true, message = result.Message });
        }
        return Json(new { success = false, message = result.Message });
    }

    #endregion
  

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
