using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Category;
using WebUI.Controllers;

namespace WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;
    public CategoryController(
    ICategoryService categoryService,
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    SignInManager<AppUser> signInManager,
    IWebHostEnvironment webHostEnvironment,
    IMapper mapper) : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
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

    #region Api GetAllCategories 
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _categoryService.GetAllCategoriesAsync();
        if (result.Success)
        {
            return Json(new
            {
                success = true,
                message = result.Message,
                result.Data
            });
        }
        return Json(new { success = false, message = result.Message });
    }
    #endregion

    #region Delete

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

    #region Create
    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<ActionResult> Create(CategoryViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _categoryService.CreateCategoryAsync(model);

        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.Message;
            return View(model);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction("Index");
    }
    #endregion
}
