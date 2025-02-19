using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;

namespace WebUI.Controllers
{
    public class HomeController:BaseController
    {

         public HomeController(
          UserManager<AppUser> userManager,
          RoleManager<AppRole> roleManager,
          SignInManager<AppUser> signInManager,
          IWebHostEnvironment webHostEnvironment,
          IMapper mapper)
        : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
            {
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
        public async Task<IActionResult> RenderProductCard(string? name, string? category, string? color, decimal? minPrice, decimal? maxPrice)
        {
            return ViewComponent("ProductCard", new { name, category, color, minPrice, maxPrice });
        }
        #endregion

        #region Details
        public IActionResult Details()
        {
            return View();
        }
        #endregion

        #region About & Contact
        public IActionResult About() => View();
        public IActionResult Contact() => View();
        #endregion

       
    }
}  