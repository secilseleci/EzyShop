using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Auth;

namespace WebUI.Controllers;
public class AuthController : BaseController
{
    private readonly ICustomerService _customerService;
    private readonly ISellerService _sellerService;

    public AuthController(
     ICustomerService customerService,
     ISellerService sellerService,
     UserManager<AppUser> userManager,
     RoleManager<AppRole> roleManager,
     SignInManager<AppUser> signInManager,
     IWebHostEnvironment webHostEnvironment,
     IMapper mapper)
   : base(userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _customerService = customerService;
        _sellerService = sellerService;
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

        var user = await UserManager.FindByEmailAsync(model.Email);
        if (user == null || user.IsDeleted)
        {
            TempData["ErrorMessage"] = Messages.LoginUserNotFound;
            return View(model);
        }

        bool isPasswordValid = await UserManager.CheckPasswordAsync(user, model.Password);
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

        if (roles.Contains("Seller"))
        {
            return RedirectToAction("Index", "Dashboard", new { area = "Seller" });
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
        return RedirectToAction("Login", "Auth", new { area = "" });
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

        if (currentUser == null || currentUser.IsDeleted)
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

        TempData["SuccessMessage"] = Messages.PasswordChangeSuccess;
        return RedirectToAction("Login", "Auth", new { area = "" });
    }
    #endregion

    #region RegisterCustomer
    [HttpGet]
    public IActionResult RegisterCustomer()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterCustomer(RegisterCustomerViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var registerResult = await CreateCustomerUserAsync(model);

        if (!registerResult.Success)
        {
            ModelState.AddModelError("", registerResult.Message);
            return View(model);
        }

        var user = registerResult.Data;

        var createCustomerResult = await _customerService.CreateCustomerAsync(user.Id, model);

        if (!createCustomerResult.Success)
        {
            ModelState.AddModelError("", createCustomerResult.Message);
            return View(model);
        }
        TempData["SuccessMessage"] = Messages.RegisterCustomerSuccess;

        return RedirectToAction("Login", "Auth", new { email = model.Email });
    }
    #endregion

    #region RegisterSeller
    [HttpGet]
    public IActionResult RegisterSeller()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterSeller(RegisterSellerViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var createSellerResult = await _sellerService.CreateSellerApplicationAsync(model);

        if (!createSellerResult.Success)
        {
            ModelState.AddModelError("", createSellerResult.Message);
            return View(model);
        }

        TempData["SuccessMessage"] = createSellerResult.Message;
        return RedirectToAction("Index", "Home");
    }
    #endregion

    #region Helpers
    private async Task<DataResult<AppUser>> CreateCustomerUserAsync(RegisterCustomerViewModel model)
    {
        var existingUser = await UserManager.FindByEmailAsync(model.Email);

        if (existingUser != null && !existingUser.IsDeleted)
        {
            return new ErrorDataResult<AppUser>(message: Messages.AlreadyExistsEmail);
        }

        var user = new AppUser
        {
            Email = model.Email,
            UserName = model.Email,
            PhoneNumber = model.Phone,
            EmailConfirmed = true
        };

        var result = await UserManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errorMessages = string.Join(" | ", result.Errors.Select(e => e.Description));
            return new ErrorDataResult<AppUser>(message: errorMessages);
        }

        if (!await RoleManager.RoleExistsAsync(CustomRoles.Customer))
            await RoleManager.CreateAsync(new AppRole { Name = CustomRoles.Customer });

        await UserManager.AddToRoleAsync(user, CustomRoles.Customer);

        return new SuccessDataResult<AppUser>(data: user);
    }
    #endregion
}
