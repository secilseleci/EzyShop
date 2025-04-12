using AutoMapper;
using Business.Services.Abstract;
using Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Models.Identity;

namespace WebUI.Controllers;

[Authorize]
public class OrderController : BaseController
{
    private readonly IOrderService _orderService;
    public OrderController(
    IOrderService orderService,
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    SignInManager<AppUser> signInManager,
    IWebHostEnvironment webHostEnvironment,
    IMapper mapper)
  : base(currentUserService, userManager, roleManager, signInManager, webHostEnvironment, mapper)
    {
        _orderService = orderService;
    }

}
