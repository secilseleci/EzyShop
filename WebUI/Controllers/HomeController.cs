using AutoMapper;
using Business.Services.Abstract;
using Business.Services.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;

namespace WebUI.Controllers
{
    public class HomeController:BaseController
    {
        private readonly IProductService _productService;

         public HomeController(
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


        #region Home Page
    public IActionResult Index()
        {
            return View();
        }
        #endregion
        
        #region Shop Page
        public IActionResult Shop()
        {
            return View();
        }
        #endregion
        
        #region ApiCall 
        [HttpGet]
        public async Task<IActionResult> GetFilteredProducts(string? name, string? category, string? color, decimal? minPrice, decimal? maxPrice)
        {
            var result = await _productService.GetFilteredProductsAsync(name, category, color, minPrice, maxPrice);

            if (!result.Success || result.Data == null || !result.Data.Any())
            {
                return Json(new { success = false, message = "No products found matching the filters." });
            }

            return Json(new { success = true, data = result.Data });
        }
        #endregion

        #region About
        public IActionResult About()
        {
            return View();
        }
        #endregion

        #region Contact
        public IActionResult Contact()
        {
            return View();
        }
        #endregion
    }
}  