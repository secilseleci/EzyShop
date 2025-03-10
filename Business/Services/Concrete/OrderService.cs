using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Models.Entities.Concrete;
using Models.Identity;
using Models.ViewModels.Order;

namespace Business.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public OrderService( 
              IOrderRepository orderRepository,
              IShoppingCartRepository shoppingCartRepository,
              IShoppingCartItemRepository shoppingCartItemRepository,
              UserManager<AppUser> userManager,
              IMapper mapper)
        {
            _orderRepository = orderRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _userManager = userManager;
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

            List<Order> orders = model.ShopOrders.Select(shopOrder => new Order
            {
                CustomerId = user.Id,
                ShopId = shopOrder.ShopId,
                OrderCode = GenerateOrderCode(),
                TotalAmount = shopOrder.TotalAmount,
                PaymentMethod = model.PaymentMethod,
                Status = Status.Pending,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                OrderItems = shopOrder.OrderItems.Select(item => new OrderItem
                {
                    ProductName = item.ProductName,
                    ProductPrice = item.ProductPrice,
                    Count = item.Count,
                    Color=item.Color,
                    ImageUrl=item.ImageUrl
                }).ToList()
            }).ToList();
 

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

    }
}
