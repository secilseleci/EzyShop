using AutoMapper;
using Business.Services.Abstract;
using Core.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models.Identity;

namespace Business.Services.Concrete;

public class OrderService : BaseService, IOrderService
{ 
    public OrderService(
    UserManager<AppUser> userManager,
    IMapper mapper,
    IConfiguration config,
    ICurrentUserService currentUser)
    : base(mapper, config, currentUser)

    { }


}
