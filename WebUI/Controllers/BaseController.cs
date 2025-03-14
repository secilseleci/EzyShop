﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Identity;
using Models.ViewModels.Abstract;
using System.Security.Claims;

namespace WebUI.Controllers
{
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
            SignInManager<AppUser> signInManager ,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper)
        {
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            RoleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            WebHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        protected async Task<AppUser?> GetCurrentUserAsync()
        {
            var user = await UserManager.Users
                .Include(u => u.Shop)
                .AsNoTracking() // Tracking olmadan getir
                .FirstOrDefaultAsync(u => u.Id == GetUserId());

            if (user == null)
            {
                Console.WriteLine("❌ Kullanıcı bulunamadı!");
            }
            else
            {
                Console.WriteLine($"✅ Kullanıcı: {user.Email}, Shop ID: {user.Shop?.Id}");
            }

            return user;
        }
        protected Guid GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = Guid.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
            return userId;
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

            model.ImageUrl = @$"images\{model.FolderName}\" + fileName;
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
}
