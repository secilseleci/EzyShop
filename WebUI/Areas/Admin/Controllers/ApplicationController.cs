using AutoMapper;
using Business.Services.Abstract;
using Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.SellerApplication;
using WebUI.Controllers;

namespace WebUI.Areas.Admin.Controllers;
[Area("Admin")]
[Route("Admin/[controller]/[action]")]
[Authorize(Roles = "Admin")]

public class ApplicationController : BaseController
{
    private readonly ISellerApplicationService _applicationService;
    public ApplicationController
    (ISellerApplicationService applicationService,
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    SignInManager<AppUser> signInManager,
    IWebHostEnvironment webHostEnvironment,
    IMapper mapper)
    : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _applicationService = applicationService;
    }



    #region List Applications

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    #endregion

    //#region API

    //[HttpGet]
    //public async Task<IActionResult> GetPaginatedApplications()
    //{
    //    int start = int.Parse(Request.Query["start"]);
    //    int length = int.Parse(Request.Query["length"]);
    //    string? search = Request.Query["search[value]"];

    //    int page = (start / length) + 1;
    //    int pageSize = length;


    //    var result = await _applicationService.GetPaginatedApplicationsAsync(page, length, search);

    //    if (!result.Success)
    //    {
    //        return Json(new { success = false, message = result.Message });
    //    }

    //    return Json(new
    //    {
    //        draw = int.Parse(Request.Query["draw"]),
    //        recordsTotal = result.Data.TotalItems,
    //        recordsFiltered = result.Data.TotalItems,
    //        data = result.Data.Items
    //    });
    //}
    //#endregion

    //#region Delete Applications
    //[HttpPost]
    //public async Task<IActionResult> Delete(Guid applicationId)
    //{

    //    var result = await _applicationService.DeleteApplicationAsync(applicationId);
    //    if (result.Success)
    //    {
    //        return Json(new { success = true, message = result.Message });
    //    }
    //    return Json(new { success = false, message = result.Message });
    //}

    //#endregion

    

    




   
}



