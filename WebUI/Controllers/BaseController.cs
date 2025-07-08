using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Abstract;
using System.Security.Claims;
namespace WebUI.Controllers;
public class BaseController : Controller
{
    protected UserManager<AppUser> UserManager { get; }
    protected RoleManager<AppRole> RoleManager { get; }
    protected SignInManager<AppUser> SignInManager { get; }
    protected IWebHostEnvironment WebHostEnvironment { get; }
    protected IMapper Mapper { get; }

    public BaseController(
     UserManager<AppUser> userManager,
     RoleManager<AppRole> roleManager,
     SignInManager<AppUser> signInManager,
     IWebHostEnvironment webHostEnvironment,
     IMapper mapper)
    {
        UserManager = userManager;
        RoleManager = roleManager;
        SignInManager = signInManager;
        WebHostEnvironment = webHostEnvironment;
        Mapper = mapper;
    }

    #region CurrentUser
    protected Guid? CurrentUserId
    {
        get
        {
            var userId = HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userId, out var id) ? id : null;
        }
    }
    protected string? CurrentUserEmail =>
        HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

    protected string? CurrentUserName =>
        HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

    protected bool IsAuthenticated =>
        HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    protected async Task<AppUser?> GetCurrentUserAsync()
    {
        if (!IsAuthenticated || !CurrentUserId.HasValue)
            return null;

        return await UserManager.FindByIdAsync(CurrentUserId.Value.ToString());
    }
  
    #endregion
     
    #region Image Upload 
    protected void HandleImageUpload(IImageViewModel model, IFormFile? file)
    {
        var wwwRootPath = WebHostEnvironment.WebRootPath;

        if (file is not null && !string.IsNullOrEmpty(model.ImageUrl))
            DeleteOldImage(model.ImageUrl, wwwRootPath);

        if (file is not null)
            CreateNewImage(model, file, wwwRootPath);
    }

    protected static void CreateNewImage(IImageViewModel model, IFormFile file, string wwwRootPath)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var imagePath = Path.Combine(wwwRootPath, "images", model.FolderName, fileName);

        using var fileStream = new FileStream(imagePath, FileMode.Create);
        file.CopyTo(fileStream);

        model.ImageUrl = "/" + Path.Combine("images", model.FolderName, fileName).Replace("\\", "/");
    }

    protected static void DeleteOldImage(string? imageUrl, string wwwRootPath)
    {
        if (string.IsNullOrEmpty(imageUrl)) { return; }

        var oldImagePath = Path.Combine(wwwRootPath, imageUrl.TrimStart('/'));
        if (System.IO.File.Exists(oldImagePath))
            System.IO.File.Delete(oldImagePath);
    }
    #endregion
}
