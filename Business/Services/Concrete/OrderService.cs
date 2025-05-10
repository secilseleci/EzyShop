using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Interfaces;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models.DTOs.OrderItem;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels.Cart;
using static Models.Entities.Concrete.OrderItem;

namespace Business.Services.Concrete;

public class OrderService : BaseService, IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IOrderItemRepository _orderItemRepo;
    private readonly IProductRepository _productRepo;
    private readonly ICustomerRepository _customerRepo;

    public OrderService(
          ICustomerRepository customerRepo,
          IProductRepository productRepo,
          IOrderRepository orderRepo,
          IOrderItemRepository orderItemRepo,
          IMapper mapper,
          IConfiguration config,
          UserManager<AppUser> userManager,
          RoleManager<AppRole> roleManager,
          ICurrentUserService currentUserService) : base(mapper, config, currentUserService)
    {
        _customerRepo = customerRepo;
        _orderRepo = orderRepo;
        _orderItemRepo = orderItemRepo;
        _productRepo = productRepo;
    }
    public async Task<IDataResult<Order?>> GetInCartOrderAsync()
    {
        if (!CurrentUserService.UserId.HasValue)
            return new ErrorDataResult<Order?>(Messages.LoginUnauthorized);

        var order = await _orderRepo.GetOrderByCustomerIdAsync(CurrentUserService.UserId.Value);

        return new SuccessDataResult<Order?>(order);

    }
    public async Task<IDataResult<Order>> AddToCartAsync(Guid productId)
    {
        if (!CurrentUserService.UserId.HasValue)
            return new ErrorDataResult<Order>(Messages.LoginUnauthorized);

        var customerId = CurrentUserService.UserId.Value;

        if (!await CheckCustomerExistsAsync(customerId))
            return new ErrorDataResult<Order>(Messages.CustomerNotFound);

        using var transaction = await _orderRepo.BeginTransactionAsync();

        var order = await GetOrCreateCartAsync(customerId);

        var itemResult = await AddOrUpdateOrderItemAsync(order.Data, productId);

        if (!itemResult.Success)
        {
            await transaction.RollbackAsync();
            return new ErrorDataResult<Order>(message: itemResult.Message);
        }

        await transaction.CommitAsync();

        return new SuccessDataResult<Order>(order.Data, message: itemResult.Message);
    }
    public async Task<IDataResult<CartPageViewModel>> GetCartPageAsync()
    {
        if (!CurrentUserService.UserId.HasValue)
            return new ErrorDataResult<CartPageViewModel>(Messages.LoginUnauthorized);

        var customerId = CurrentUserService.UserId.Value;

        if (!await _customerRepo.ExistsAsync(c => c.Id == customerId))
            return new ErrorDataResult<CartPageViewModel>(Messages.CustomerNotFound);

        var order = await _orderRepo.GetOrderByCustomerIdAsync(customerId);
        if (order == null || !order.OrderItems.Any())
        {
            return new SuccessDataResult<CartPageViewModel>(new CartPageViewModel { OrderItems = new List<OrderItemDto>() });
        }

        var items = await _orderItemRepo.GetOrderItemsAsync(order.Id);

        var total = items.Sum(x => x.Count * x.ProductPrice);

        var vm = new CartPageViewModel
        {
            TotalAmount = total,
            OrderItems = items.ToList()
        };

        return new SuccessDataResult<CartPageViewModel>(vm);
    }


    private async Task<bool> CheckCustomerExistsAsync(Guid customerId)
  => await _customerRepo.ExistsAsync(c => c.Id == customerId);
    private async Task<IDataResult<Order>> GetOrCreateCartAsync(Guid customerId)
    {

        var existingOrder = await _orderRepo.GetOrderByCustomerIdAsync(customerId);
        if (existingOrder != null)
            return new SuccessDataResult<Order>(existingOrder);

        var newOrder = await _orderRepo.CreateOrderAsync(customerId);
        if (newOrder == null)
            return new ErrorDataResult<Order>(message: Messages.CreateError);

        return new SuccessDataResult<Order>(newOrder);
    }
    private async Task<IResult> AddOrUpdateOrderItemAsync(Order order, Guid productId)
    {
        var orderItemResult = await _orderItemRepo.GetOrderItemByOrderandProductId(order.Id, productId);
        if (orderItemResult != null)
        {
            orderItemResult.Count += 1;
            var updateResult = await _orderItemRepo.UpdateAsync(orderItemResult);
            if (updateResult <= 0)
            {
                return new ErrorResult(message: Messages.UpdateError);
            }
        }
        else
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null || product.Stock <= 0)
                return new ErrorResult(Messages.ProductNotFound);

            var neworderItem = new OrderItem
            {
                ProductId = productId,
                OrderId = order.Id,
                Count = 1,
                ProductName = product.Name,
                ProductPrice = product.Price,
                Color = product.Color,
                ImageUrl = product.ImageUrl,
                Status = OrderItemStatus.InCart,
            };

            var createResult = await _orderItemRepo.CreateAsync(neworderItem);
            if (createResult <= 0)
                return new ErrorResult(message: Messages.CreateError);
        }

        return new SuccessResult(message: Messages.ProductAddedSuccess);
    }
}
