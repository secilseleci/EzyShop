using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController:BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(
        ICategoryService categoryService,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
         : base(webHostEnvironment: webHostEnvironment, mapper: mapper)
        {
            _categoryService = categoryService;
        }

        #region List Categories

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync(c => true);
            return Json(new { data = categories.Data });

        }
        #endregion

        #region Create Categories
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model, IFormFile? file)
        {
           
            HandleImageUpload(model, file);
            var result=await _categoryService.CreateCategoryAsync(model);
            if (!result.Success)
            {
                TempData["ErrorMessage"]=result.Message;
            }

            TempData["SuccessMessage"]=result.Message;
            return RedirectToAction("Index");
        }
        #endregion



        #region Delete Categories
        public async Task<IActionResult> Delete(Guid id)
        {
            var categoryResult = await _categoryService.GetCategoryByIdAsync(id);
            if (!categoryResult.Success)
            {
                return Json(categoryResult);
            }
            var result = await _categoryService.DeleteCategoryAsync(id);
            if(!result.Success)
            {
                
                return Json(result);
            }
            DeleteOldImage(categoryResult.Data.ImageUrl, WebHostEnvironment.WebRootPath);
            return Json(result);
         }

        #endregion
    }
}
