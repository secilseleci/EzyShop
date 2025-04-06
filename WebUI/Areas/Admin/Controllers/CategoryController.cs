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

public class CategoryController :  BaseController
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
    public IActionResult Index()
    {
        return View();
    }


    //        #region List Categories

    //        [HttpGet]
    //        public IActionResult Index()
    //        {
    //            return View();
    //        }
    //        [HttpGet]
    //        public async Task<IActionResult> GetAll()
    //        {
    //            var categories = await _categoryService.GetAllCategoriesAsync(c => true);
    //            return Json(new { data = categories.Data });

    //        }
    //        #endregion

    //        #region Create Categories
    //        [HttpGet]
    //        public IActionResult Create()
    //        {
    //            return View();
    //        }

    //        [HttpPost]
    //        public async Task<IActionResult> Create(CategoryViewModel model, IFormFile? file)
    //        {

    //            HandleImageUpload(model, file);
    //            var result=await _categoryService.CreateCategoryAsync(model);
    //            if (!result.Success)
    //            {
    //                TempData["ErrorMessage"]=result.Message;
    //            }

    //            TempData["SuccessMessage"]=result.Message;
    //            return RedirectToAction("Index");
    //        }
    //        #endregion

    //        #region Delete Categories
    //        public async Task<IActionResult> Delete(Guid id)
    //        {
    //            var categoryResult = await _categoryService.GetCategoryByIdAsync(id);
    //            if (!categoryResult.Success)
    //            {
    //                return Json(categoryResult);
    //            }
    //            var result = await _categoryService.DeleteCategoryAsync(id);
    //            if(!result.Success)
    //            {

    //                return Json(result);
    //            }
    //            DeleteOldImage(categoryResult.Data.ImageUrl, WebHostEnvironment.WebRootPath);
    //            return Json(result);
    //         }

    //        #endregion

    //        #region Edit Cetagories
    //        [HttpGet]
    //        public async Task<IActionResult> Edit(Guid id)
    //        {
    //            var result = await _categoryService.GetCategoryByIdAsync(id);
    //            if (!result.Success)
    //            {
    //                TempData["ErrorMessage"] = result.Message;
    //                return View();
    //            }

    //            TempData["SuccessMessage"] = result.Message;
    //            return View(Mapper.Map<CategoryViewModel>(result.Data));
    //        }
    //        [HttpPost]
    //        public async Task<IActionResult>Edit(CategoryViewModel model, IFormFile? file)
    //        {
    //            HandleImageUpload(model, file);

    //            var result = await _categoryService.UpdateCategoryAsync(model);
    //            if (!result.Success)
    //            {
    //                TempData["ErrorMessage"] = result.Message;
    //                return View(model);
    //            }

    //            TempData["SuccessMessage"] = result.Message;
    //            return RedirectToAction(nameof(Index));
    //        }
    //        #endregion
    //    }
    //}

}
