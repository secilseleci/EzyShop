using AutoMapper;
using Core.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Identity;
using Models.ViewModels.Abstract;
namespace WebUI.Controllers;
public class BaseController : Controller
{
    protected UserManager<AppUser> UserManager { get; }
    protected RoleManager<AppRole> RoleManager { get; }
    protected SignInManager<AppUser> SignInManager { get; }
    protected ICurrentUserService CurrentUserService { get; }
    protected IWebHostEnvironment WebHostEnvironment { get; }
    protected IMapper Mapper { get; }

    public BaseController(
     ICurrentUserService currentUserService,
     UserManager<AppUser> userManager,
     RoleManager<AppRole> roleManager,
     SignInManager<AppUser> signInManager,
     IWebHostEnvironment webHostEnvironment,
     IMapper mapper)
    {
        CurrentUserService = currentUserService;
        UserManager = userManager;
        RoleManager = roleManager;
        SignInManager = signInManager;
        WebHostEnvironment = webHostEnvironment;
        Mapper = mapper;
    }

  

    protected Guid? GetUserId() => CurrentUserService.UserId;

    protected async Task<AppUser?> GetCurrentUserAsync()
    {
        

        if (!CurrentUserService.UserId.HasValue)
            return null;

        return await UserManager.Users
            .FirstOrDefaultAsync(u => u.Id == CurrentUserService.UserId.Value);
    }

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
