using AutoMapper;
using Business.Services.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Models.Identity;
using Models.ViewModels;

namespace Business.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public OrderService(
          IShoppingCartRepository shoppingCartRepository,
          IShoppingCartItemRepository shoppingCartItemRepository,
          UserManager<AppUser> userManager,
          IMapper mapper)
        {
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
    }
}
