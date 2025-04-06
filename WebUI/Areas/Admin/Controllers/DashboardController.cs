using AutoMapper;
using Business.Services.Abstract;
using Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Concrete;
using Models.Identity;
using System.Linq.Expressions;
using WebUI.Controllers;

namespace WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/[controller]/[action]")]
[Authorize(Roles = "Admin")]
public class DashboardController : BaseController
{
    private readonly ISellerApplicationService _sellerApplicationService;
    private readonly IEmailService _emailService;


    public DashboardController
    (
        ICurrentUserService currentUserService,
        ISellerApplicationService sellerApplicationService,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,
        IEmailService emailService,
        IMapper mapper)
        : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _sellerApplicationService = sellerApplicationService;
        _emailService = emailService;

    }
    public IActionResult Index()
    {
        return View();
     }

    

   
}
