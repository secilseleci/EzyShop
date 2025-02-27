using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Controllers
{
    public class ApplicationController:BaseController
    {            
        private readonly ISellerApplicationService _sellerApplicationService;
        public ApplicationController(
           ISellerApplicationService sellerApplicationService, 
           UserManager<AppUser> userManager,
           RoleManager<AppRole> roleManager,
           SignInManager<AppUser> signInManager,
           IWebHostEnvironment webHostEnvironment,
           IMapper mapper)
           : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
        {            _sellerApplicationService = sellerApplicationService;

        }
     

        #region Become Seller/ Create Application

        [HttpGet]
        public IActionResult BecomeSeller()
        {
            return View(new SellerRegistrationViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> BecomeSeller(SellerRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
          
            var result = await _sellerApplicationService.CreateSellerApplicationAsync(model);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
          
            return RedirectToAction("Index", "Home");
        }

        #endregion
        [HttpGet]
        public async Task<IActionResult> CheckEmailAvailability(string email)
        {
            var existingUser = await UserManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return Json(new { available = false, message = "This email is already registered. Please use another email for your seller application." });
            }

            return Json(new { available = true });
        }



    }
}
