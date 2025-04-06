using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Security;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels.Order;

namespace Business.Services.Concrete;

public class OrderService : BaseService, IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IProductRepository _productRepo;
    private readonly IShoppingCartRepository _shoppingCartRepo;
    private readonly IShoppingCartItemRepository _shoppingCartItemRepo;
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;

    public OrderService(
          IOrderRepository orderRepo,
          IProductRepository productRepo,
          IShoppingCartRepository shoppingCartRepo,
          IShoppingCartItemRepository shoppingCartItemRepo,
          UserManager<AppUser> userManager,
          IEmailService emailService,
          IMapper mapper,
            IConfiguration config,
            ICurrentUserService currentUser)
         : base(mapper, config, currentUser)
    {
        _orderRepo = orderRepo;
        _productRepo = productRepo;
        _shoppingCartRepo = shoppingCartRepo;
        _shoppingCartItemRepo = shoppingCartItemRepo;
        _userManager = userManager;
        _emailService = emailService;

    }

    public Task<IDataResult<List<string>>> CreateOrderAsync(SummaryViewModel model)
    {
        throw new NotImplementedException();
    }

    public Task<IDataResult<IEnumerable<OrderViewModel>>> GetAllOrdersAsync(Guid shopId)
    {
        throw new NotImplementedException();
    }

    public Task<IDataResult<OrderViewModel>> GetOrderByIdAsync(Guid orderId)
    {
        throw new NotImplementedException();
    }

    public Task<IDataResult<SummaryViewModel>> GetOrderSummaryAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IDataResult<OrderViewModel>> MarkAsShipped(Guid orderId)
    {
        throw new NotImplementedException();
    }

    //public async Task<IDataResult<SummaryViewModel>> GetOrderSummaryAsync(Guid userId)
    //{
    //    var cart = await _shoppingCartRepo.GetCartByUserIdAsync(userId);
    //    if (cart == null)
    //        return new ErrorDataResult<SummaryViewModel>(Messages.ShoppingCartNotFound);

    //    var cartItems = await _shoppingCartItemRepo.GetCartItemsAsync(cart.Id);
    //    if (!cartItems.Any())
    //        return new ErrorDataResult<SummaryViewModel>(Messages.ShoppingCartEmpty);

    //    var orderItems = Mapper.Map<List<OrderItemViewModel>>(cartItems);

    //    var shopOrders = orderItems.GroupBy(i => i.ShopId)
    //        .Select(group => new ShopOrderViewModel
    //        {
    //            ShopId = group.Key,
    //            ShopName = group.First().ShopName,
    //            OrderItems = group.ToList()
    //        }).ToList();

    //    var user = await _userManager.FindByIdAsync(userId.ToString());
    //    if (user == null)
    //        return new ErrorDataResult<SummaryViewModel>(Messages.UserNotFound);

    //    var summary = Mapper.Map<SummaryViewModel>(user);
    //    summary.ShopOrders = shopOrders;

    //    return new SuccessDataResult<SummaryViewModel>(summary, "Order summary retrieved successfully.");
    //}
    //public async Task<IDataResult<List<string>>> CreateOrderAsync(SummaryViewModel model)
    //{

    //    var user = await _userManager.FindByIdAsync(model.CustomerId.ToString());
    //    if (user == null)
    //        return new ErrorDataResult<List<string>>(Messages.UserNotFound);

    //    var cart = await _shoppingCartRepo.GetCartByUserIdAsync(user.Id);
    //    if (cart == null)
    //        return new ErrorDataResult<List<string>>(Messages.ShoppingCartNotFound);

    //    var cartItems = await _shoppingCartRepo.GetCartItemsAsync(cart.Id);
    //    if (!cartItems.Any())
    //        return new ErrorDataResult<List<string>>(Messages.ShoppingCartEmpty);

    //    var orders = Mapper.Map<List<Order>>(model.ShopOrders);

    //    foreach (var order in orders)
    //    {
    //        order.CustomerId = user.Id;
    //        order.OrderCode = GenerateOrderCode();
    //        order.PaymentMethod = model.PaymentMethod;
    //        order.Status = Status.Pending;
    //        order.CreatedDate = DateTime.UtcNow;
    //        order.UpdatedDate = DateTime.UtcNow;
    //        foreach (var item in order.OrderItems)
    //        {
    //            var cartItem = cartItems.FirstOrDefault(c => c.ProductId == item.ProductId);
    //            if (cartItem == null || cartItem.Product == null)
    //                return new ErrorDataResult<List<string>>($"Product {item.ProductName} not found.");

    //            var product = cartItem.Product;

    //            if (product.Stock < item.Count)
    //                return new ErrorDataResult<List<string>>($"Not enough stock for {item.ProductName}.");

    //            product.Stock -= item.Count;
    //            await _productRepo.UpdateAsync(product);
    //        }

    //    }

    //    await _orderRepo.AddRangeAsync(orders);

    //    await _shoppingCartItemRepo.DeleteRangeAsync(cartItems);

    //    return new SuccessDataResult<List<string>>(orders.Select(o => o.OrderCode).ToList(), Messages.CreateOrderSuccess);
    //}
    //private string GenerateOrderCode()
    //{
    //    return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
    //}

    //public async Task<IDataResult<IEnumerable<OrderViewModel>>> GetAllOrdersAsync(Guid shopId)
    //{
    //    var orders = await _orderRepo.GetOrdersWithDetailsAsync(shopId);

    //    if (!orders.Any())
    //        return new ErrorDataResult<IEnumerable<OrderViewModel>>(Messages.EmptyOrderList);

    //    var orderViewModels = Mapper.Map<IEnumerable<OrderViewModel>>(orders);

    //    return new SuccessDataResult<IEnumerable<OrderViewModel>>(orderViewModels);
    //}
    //public async Task<IDataResult<OrderViewModel>> GetOrderByIdAsync(Guid orderId)
    //{
    //    var order = await _orderRepo.GetOrderWithDetailsAsync(orderId);
    //    if (order == null)
    //        return new ErrorDataResult<OrderViewModel>(Messages.OrderNotFound);

    //    var orderViewModel = Mapper.Map<OrderViewModel>(order);
    //    return new SuccessDataResult<OrderViewModel>(orderViewModel);
    //}


    //public async Task<IDataResult<OrderViewModel>> MarkAsShipped(Guid orderId)
    //{
    //    var order = await _orderRepo.GetOrderWithCustomerAsync(orderId);
    //    if (order == null)
    //        return new ErrorDataResult<OrderViewModel>(Messages.OrderNotFound);

    //    if (order.Status != Status.Pending)
    //        return new ErrorDataResult<OrderViewModel>(Messages.OrderStatusIsNotPending);

    //    order.Status = Status.Shipped;
    //    order.ShippingTrackingNumber = GenerateTrackingNumber();
    //    await _orderRepo.UpdateAsync(order);

    //    var customerEmail = order.Customer?.Email;
    //    if (string.IsNullOrEmpty(customerEmail))
    //        return new ErrorDataResult<OrderViewModel>(Messages.CustomerEmailError);

    //    await _emailService.SendEmailAsync(customerEmail,
    //        "Your order has been shipped!",
    //        $"Your tracking number is {order.ShippingTrackingNumber}");


    //    return new SuccessDataResult<OrderViewModel>(Mapper.Map<OrderViewModel>(order), "Order marked as shipped.");
    //}
    //private string GenerateTrackingNumber()
    //{
    //    return $"TRK-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(100000, 999999)}";
    //}

}
