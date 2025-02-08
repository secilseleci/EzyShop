using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;

namespace WebUI.Controllers
{
    public class HomeController:BaseController
    {
        public HomeController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, IWebHostEnvironment webHostEnvironment, IMapper mapper) : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
        {
        }


        #region Home Page
        public IActionResult Index()
        {
            return View();
        }
        #endregion
        #region Home Page
        public IActionResult ProductList()
        {
            return View();
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