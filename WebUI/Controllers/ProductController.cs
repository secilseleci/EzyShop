using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Product;

namespace WebUI.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IShopService _shopService;

        public ProductController(
              IProductService productService,
              IShopService shopService,

              UserManager<AppUser> userManager,
              RoleManager<AppRole> roleManager,
              SignInManager<AppUser> signInManager,
              IWebHostEnvironment webHostEnvironment,
              IMapper mapper)
      : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
        {
            _productService = productService;
            _shopService = shopService;
        }

        #region Read
        [Authorize(Roles = "Seller")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Create
        [Authorize(Roles = "Seller")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "Account");

            var shopResult = await _shopService.GetShopBySellerIdAsync(user.Id);
            if (!shopResult.Success || shopResult.Data == null)
            {
                TempData["ErrorMessage"] = "You don't have an active shop. Please contact admin.";
                return RedirectToAction(nameof(Index));
            }

            var model = new ProductViewModel
            {
                ShopId = shopResult.Data.Id  
            };

            return View(model);
        }
        [Authorize(Roles = "Seller")]
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model, IFormFile? file)
        {
            if (!ModelState.IsValid) return View(model);
            HandleImageUpload(model, file);

            var result = await _productService.CreateProductAsync(model);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return View();
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        [Authorize(Roles = "Seller")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return View();
            }

            TempData["SuccessMessage"] = result.Message;
            return View(Mapper.Map<ProductViewModel>(result.Data));
        }

        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Edit(ProductViewModel model, IFormFile? file)
        {
            HandleImageUpload(model, file);

            var result = await _productService.UpdateProductAsync(model);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(model);
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction("Index");
        }
        #endregion

        #region API
        [HttpGet]
        public async Task<IActionResult> GetSellerProducts()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized();

            var shopResult = await _shopService.GetShopBySellerIdAsync(user.Id);
            if (!shopResult.Success || shopResult.Data == null)
            {
                return Json(new { data = new List<object>() });  
            }

            var products = await _productService.GetAllProductsWithCategoryAsync(p => p.ShopId == shopResult.Data.Id);
            if (products.Data == null || !products.Data.Any())
            {
                return Json(new { data = new List<object>() });
            }

            return Json(new { data = products.Data });
        }


        [Authorize(Roles = "Seller")]
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetFilteredProducts(string? name, string? category, string? color, decimal? minPrice, decimal? maxPrice)
        {
            var result = await _productService.GetFilteredProductsAsync(name, category, color, minPrice, maxPrice);

            if (!result.Success || result.Data == null || !result.Data.Any())
            {
                return NotFound(new { success = false, message = "No products found matching the filters." });
            }

            return Json(new { success = true, data = result.Data });
        }
        #endregion

        #region Product Details
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> Details(Guid productId)
        {
            var result = await _productService.GetProductByIdWithCategoryAsync(productId);
            if (!result.Success || result.Data == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction("Shop", "Home");  
            }

            return View(result.Data); 
        }
        #endregion
    }
}
