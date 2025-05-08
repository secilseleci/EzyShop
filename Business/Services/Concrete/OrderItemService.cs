using AutoMapper;
using Business.Services.Abstract;
using Core.Interfaces;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models.Identity;

namespace Business.Services.Concrete;

public class OrderItemService : BaseService, IOrderItemService
{
    private readonly IOrderItemRepository _orderItemRepo;
    public OrderItemService(
      IOrderItemRepository orderItemRepo,
      IMapper mapper,
      IConfiguration config,
      UserManager<AppUser> userManager,
      RoleManager<AppRole> roleManager,
      ICurrentUserService currentUserService,
      ICategoryRepository categoryRepo) : base(mapper, config, currentUserService)
    {
        _orderItemRepo = orderItemRepo;
    }
}
