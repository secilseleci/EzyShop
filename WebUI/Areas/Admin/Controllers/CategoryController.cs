using AutoMapper;
using Business.Services.Abstract;
using Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Category;
using WebUI.Controllers;

namespace WebUI.Areas.Admin.Controllers;
[Area("Admin")]
[Route("Admin/[controller]/[action]")]
[Authorize(Roles = "Admin")]

public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;
    public CategoryController
    (ICategoryService categoryService,
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    SignInManager<AppUser> signInManager,
    IWebHostEnvironment webHostEnvironment,
    IMapper mapper)
    : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _categoryService = categoryService;
    }



    #region List Categories

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    #endregion

    #region API

    [HttpGet]
    public async Task<IActionResult> GetPaginatedCategories()
    {
        int start = int.Parse(Request.Query["start"]);
        int length = int.Parse(Request.Query["length"]);
        string? search = Request.Query["search[value]"];

        int page = (start / length) + 1;
        int pageSize = length;


        var result = await _categoryService.GetPaginatedCategoriesAsync(page, length, search);

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

    #region Delete Categories
    [HttpPost]
    public async Task<IActionResult> Delete(Guid categoryId)
    {

        var result = await _categoryService.DeleteCategoryAsync(categoryId);
        if (result.Success)
        {
            return Json(new { success = true, message = result.Message });
        }
        return Json(new { success = false, message = result.Message });
    }

    #endregion

    #region Create Category
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryViewModel model, IFormFile? file)
    {

        HandleImageUpload(model, file);
        var result = await _categoryService.CreateCategoryAsync(model);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction("Index");
    }
    #endregion



    #region Edit Cetagory
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (!category.Success)
        {
            TempData["ErrorMessage"] = category.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(Mapper.Map<CategoryViewModel>(category.Data));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryViewModel model, IFormFile? file)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        HandleImageUpload(model, file);

        var result = await _categoryService.UpdateCategoryAsync(model);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(model);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Index));
    }
    #endregion
}



