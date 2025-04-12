using AutoMapper;
using Business.Services.Abstract;
using Core.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Models.ViewModels.Shop;

namespace WebUI.Controllers;
 
public class ShopController : BaseController
{
    private readonly IShopService _shopService;

    public ShopController(
          ICurrentUserService currentUserService,
          IShopService shopService,
          UserManager<AppUser> userManager,
          RoleManager<AppRole> roleManager,
          SignInManager<AppUser> signInManager,
          IWebHostEnvironment webHostEnvironment,
          IMapper mapper)
    : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
       
    {
        _shopService = shopService;
    }
  
    
    

  
   
}
