using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Seller")]

    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        public ProductController(
              IProductService productService,
              UserManager<AppUser> userManager,
              RoleManager<AppRole> roleManager,
              SignInManager<AppUser> signInManager,
              IWebHostEnvironment webHostEnvironment,
              IMapper mapper)
      : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
        {
            _productService = productService;
        }
        #region Read

        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model, IFormFile? file)
        {
            HandleImageUpload(model, file);

            var result = await _productService.CreateProductAsync(model);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return View();
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProductsWithCategoryAsync(c => true);
            return Json(new { data = products.Data });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var productResult = await _productService.GetProductByIdAsync(id);
            if (!productResult.Success)
            {
                return Json(productResult);
            }

            var deleteResult = await _productService.DeleteProductAsync(id);
            if (!deleteResult.Success)
            {
                TempData["ErrorMessage"] = deleteResult.Message;
                return Json(deleteResult);
            }

            DeleteOldImage(productResult.Data.ImageUrl, WebHostEnvironment.WebRootPath);
            TempData["SuccessMessage"] = deleteResult.Message;
            return Json(deleteResult);
        }
    }
}
