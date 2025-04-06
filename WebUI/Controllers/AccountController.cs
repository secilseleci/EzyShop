using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.User;

namespace WebUI.Controllers;

public class AccountController : BaseController
{
    private readonly IAuthService _authService;
    public AccountController(
        ICurrentUserService currentUserService,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IAuthService authService,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
        : base(currentUserService,userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _authService = authService;
    }


    #region Login
    [HttpGet]
    public IActionResult Login(string email = "", string returnUrl = "")
    {
        if (!string.IsNullOrEmpty(returnUrl))
        {
            TempData["ReturnUrl"] = returnUrl;
        }

        return View(new LoginViewModel { Email = email });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    { 

        var user = await _authService.FindByEmailAsync(model.Email);
        if (user is null)
        {
            TempData["ErrorMessage"] = Messages.LoginUserNotFound;
            return View(model);
        }

        bool isPasswordValid = await _authService.CheckPasswordAsync(user, model.Password);
        if (!isPasswordValid)
        {
            TempData["ErrorMessage"] = Messages.LoginInvalidCredentials;
            return View(model);
        }

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = false,
            ExpiresUtc = null  
        };

        await SignInManager.SignInAsync(user, authProperties);


        var roles = await UserManager.GetRolesAsync(user);
        if (roles.Contains("Admin"))
        {
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        
        TempData["SuccessMessage"] = Messages.LoginSuccess;
        return RedirectToAction("Index", "Home");
      
    }

    #endregion

    #region Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {

        await SignInManager.SignOutAsync();
        TempData["SuccessMessage"] = Messages.LogoutSuccess;
        return RedirectToAction("Login", "Account", new { area = "" });
    }
    #endregion

    #region Change Password
    [Authorize]
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        
        var currentUser = await GetCurrentUserAsync();
        if (currentUser is null)
        {
            TempData["ErrorMessage"] = Messages.UserNotFound;
            return View();
        }
        bool oldPasswordCorrect = await UserManager.CheckPasswordAsync(currentUser, model.OldPassword);
        if (!oldPasswordCorrect)
        {
            TempData["ErrorMessage"] = Messages.OldPasswordError;
            return View(model);
        }
        IdentityResult result = await UserManager.ChangePasswordAsync(currentUser, model.OldPassword, model.NewPassword);
        if (!result.Succeeded)
        {
            TempData["ModelError"] = result.Errors.Select(e => e.Description).ToList();
            return View(model);
        }

        await UserManager.UpdateSecurityStampAsync(currentUser);
        await SignInManager.SignOutAsync();
        await SignInManager.PasswordSignInAsync(currentUser, model.NewPassword, true, false);

        TempData["SuccessMessage"] = Messages.PasswordChangeSuccess;
        return RedirectToAction("ChangePassword");

    }
    #endregion


}

