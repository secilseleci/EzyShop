using AutoMapper;
using Core.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels;

namespace WebUI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly List<string> ErrorList = new();

        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, IWebHostEnvironment webHostEnvironment, IMapper mapper) : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
        {
        }
         

        #region Login
        [HttpGet]
        public IActionResult Login(string email, string returnUrl)
        {
            ModelState.Clear();
            if (!string.IsNullOrEmpty(returnUrl))
            {
                TempData["ReturnUrl"] = returnUrl;
            }
            return email is null
                ? View()
                : View(new LoginViewModel { Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ErrorList.Clear();
            TempData["ModelError"] = ErrorList;

            if (!ModelState.IsValid)
            {
                return AddModelErrorsAndSendToClient(loginModel: model);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Başarıyla giriş yaptınız.";
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                TempData["ErrorMessage"] = "Hesabınız çok fazla yanlış deneme nedeniyle kilitlendi.";
                return View(model);
            }

            TempData["ErrorMessage"] = "E-posta veya şifre yanlış.";
            return View(model);
        }
        #endregion

        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            TempData["ModelError"] = ErrorList;

            if (!ModelState.IsValid)
            {
                return AddModelErrorsAndSendToClient(registerModel: model);
            }

            AppUser user = Mapper.Map<AppUser>(model);
            var result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return AddRegisterErrorsAndSendToClient(model, result);
            }

            await UserManager.AddToRoleAsync(user, "Customer");
            TempData["SuccessMessage"] = "Register Successfull";
            return RedirectToAction(nameof(Login), routeValues: new { email = model.Email, returnUrl = string.Empty });
        }
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            TempData["SuccessMessage"] = "Başarıyla çıkış yaptınız.";
            return RedirectToAction("Login");
        }
        #endregion
      
        #region Change Password
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) 
            {
                TempData["ModelError"] = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
                return View(model);
            }
            if (CurrentUser is null)
            {
                TempData["ErrorMessage"] = Messages.UserNotFound;
                return View();
            }
            bool oldPasswordCorrect=await UserManager.CheckPasswordAsync(CurrentUser,model.OldPassword);
            if(!oldPasswordCorrect)
            {
                TempData["ErrorMessage"] = "Old password is wrong";
                return View(model);
            }
            IdentityResult result=await UserManager.ChangePasswordAsync(CurrentUser,model.OldPassword,model.NewPassword);
            if (!result.Succeeded)
            {
                TempData["ModelError"] = result.Errors.Select(e => e.Description).ToList();
                return View(model);
            }

                await UserManager.UpdateSecurityStampAsync(CurrentUser);
                await SignInManager.SignOutAsync();
                await SignInManager.PasswordSignInAsync(CurrentUser, model.NewPassword, true, false);

                TempData["SuccessMessage"] = "Password successfully changed";
                return RedirectToAction("ChangePassword");  



            }
        #endregion

     

        private IActionResult AddModelErrorsAndSendToClient(LoginViewModel? loginModel = null, RegisterViewModel? registerModel = null)
        {
            var errorMessagesFinal = ModelState.Values
                .SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            TempData["ModelError"] = errorMessagesFinal;

            return loginModel is not null ? View(loginModel) : View(registerModel);
        }

        private IActionResult AddRegisterErrorsAndSendToClient(RegisterViewModel model, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ErrorList.Add(error.Description);
            }
            return View(model);
        }
    }
}
