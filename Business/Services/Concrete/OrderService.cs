using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using DataAccess.Repositories.Concrete;
using Microsoft.AspNetCore.Identity;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels.Order;

namespace Business.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper; 
        private readonly IEmailService _emailService;

        public OrderService( 
              IOrderRepository orderRepository,
              IProductRepository productRepository,
              IShoppingCartRepository shoppingCartRepository,
              IShoppingCartItemRepository shoppingCartItemRepository,
              UserManager<AppUser> userManager, 
              IEmailService emailService,
              IMapper mapper
             )
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _userManager = userManager;
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<IDataResult<SummaryViewModel>> GetOrderSummaryAsync(Guid userId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                return new ErrorDataResult<SummaryViewModel>(Messages.ShoppingCartNotFound);

            var cartItems = await _shoppingCartItemRepository.GetCartItemsAsync(cart.Id);
            if (!cartItems.Any())
                return new ErrorDataResult<SummaryViewModel>(Messages.ShoppingCartEmpty);

            var orderItems = _mapper.Map<List<OrderItemViewModel>>(cartItems);

            var shopOrders = orderItems.GroupBy(i => i.ShopId)
                .Select(group => new ShopOrderViewModel
                {
                    ShopId = group.Key,
                    ShopName = group.First().ShopName,
                    OrderItems = group.ToList()
                }).ToList();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new ErrorDataResult<SummaryViewModel>(Messages.UserNotFound);

            var summary = _mapper.Map<SummaryViewModel>(user);
            summary.ShopOrders = shopOrders;

            return new SuccessDataResult<SummaryViewModel>(summary, "Order summary retrieved successfully.");
        }
        public async Task<IDataResult<List<string>>> CreateOrderAsync(SummaryViewModel model)
        {
             
            var user = await _userManager.FindByIdAsync(model.CustomerId.ToString());
            if (user == null)
                return new ErrorDataResult<List<string>>(Messages.UserNotFound);

            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(user.Id);
            if (cart == null)
                return new ErrorDataResult<List<string>>(Messages.ShoppingCartNotFound);

            var cartItems = await _shoppingCartItemRepository.GetCartItemsAsync(cart.Id);
            if (!cartItems.Any())
                return new ErrorDataResult<List<string>>(Messages.ShoppingCartEmpty);

            var orders = _mapper.Map<List<Order>>(model.ShopOrders);

            foreach (var order in orders)
            {
                order.CustomerId = user.Id;
                order.OrderCode = GenerateOrderCode();
                order.PaymentMethod = model.PaymentMethod;
                order.Status = Status.Pending;
                order.CreatedDate = DateTime.UtcNow;
                order.UpdatedDate = DateTime.UtcNow;
                foreach (var item in order.OrderItems)
                {
                    var cartItem = cartItems.FirstOrDefault(c => c.ProductId == item.ProductId);
                    if (cartItem == null || cartItem.Product == null)
                        return new ErrorDataResult<List<string>>($"Product {item.ProductName} not found.");

                    var product = cartItem.Product;  

                    if (product.Stock < item.Count)
                        return new ErrorDataResult<List<string>>($"Not enough stock for {item.ProductName}.");

                    product.Stock -= item.Count;
                    await _productRepository.UpdateAsync(product);
                }

            }

            await _orderRepository.AddRangeAsync(orders);

            await _shoppingCartItemRepository.DeleteRangeAsync(cartItems);

            return new SuccessDataResult<List<string>>(orders.Select(o => o.OrderCode).ToList(), Messages.CreateOrderSuccess);
        }
        private string GenerateOrderCode()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
        }

        public async Task<IDataResult<IEnumerable<OrderViewModel>>> GetAllOrdersAsync(Guid shopId)
        {
            var orders = await _orderRepository.GetOrdersWithDetailsAsync(shopId);

            if (!orders.Any())
                return new ErrorDataResult<IEnumerable<OrderViewModel>>(Messages.EmptyOrderList);

            var orderViewModels = _mapper.Map<IEnumerable<OrderViewModel>>(orders);

            return new SuccessDataResult<IEnumerable<OrderViewModel>>(orderViewModels);
        }
        public async Task<IDataResult<OrderViewModel>> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            if (order == null)
                return new ErrorDataResult<OrderViewModel>(Messages.OrderNotFound);

            var orderViewModel = _mapper.Map<OrderViewModel>(order);
            return new SuccessDataResult<OrderViewModel>(orderViewModel);
        }

    
        public async Task<IDataResult<OrderViewModel>>MarkAsShipped(Guid orderId)
        {
            var order = await _orderRepository.GetOrderWithCustomerAsync(orderId);
            if (order == null)
                return new ErrorDataResult<OrderViewModel>(Messages.OrderNotFound);

            if (order.Status != Status.Pending)
                return new ErrorDataResult<OrderViewModel>(Messages.OrderStatusIsNotPending);
            
            order.Status = Status.Shipped;
            order.ShippingTrackingNumber= GenerateTrackingNumber();
            await _orderRepository.UpdateAsync(order);
            
            var customerEmail=order.Customer?.Email;
            if (string.IsNullOrEmpty(customerEmail))
                return new ErrorDataResult<OrderViewModel>(Messages.CustomerEmailError);

            await _emailService.SendEmailAsync(customerEmail,
                "Your order has been shipped!", 
                $"Your tracking number is {order.ShippingTrackingNumber}");

            
            return new SuccessDataResult<OrderViewModel>(_mapper.Map<OrderViewModel>(order), "Order marked as shipped.");
        }
        private string GenerateTrackingNumber()
        {
            return $"TRK-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(100000, 999999)}";
        }

    }
}
