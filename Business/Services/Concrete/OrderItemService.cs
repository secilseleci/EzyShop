using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Interfaces;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models.DTOs.OrderItem;
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

    public async Task<IEnumerable<OrderItemDto>> GetOrderItemsAsync(Guid orderId)
    {
        return await _orderItemRepo.GetOrderItemsAsync(orderId);
    }

    public async Task<IResult> DeleteItemsAsync(Guid orderId)
    {
        var items = await _orderItemRepo.GetByOrderIdAsync(orderId);
        if (!items.Any()) return new SuccessResult(Messages.EmptyEntityList);

        var deleteResult = await _orderItemRepo.SoftDeleteRangeAsync(items);

        return deleteResult > 0
            ? new SuccessResult(Messages.DeleteSuccess)
        : new ErrorResult(Messages.DeleteError);
    }

}
