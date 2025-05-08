using Business.Services.Abstract;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.ViewComponents;

public class CartIconViewComponent:ViewComponent
{
    private readonly IOrderService _orderService;
    private readonly ICurrentUserService _currentUser;

    public CartIconViewComponent(IOrderService orderService, ICurrentUserService currentUser)
    {
        _orderService = orderService;
        _currentUser = currentUser;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        int count = 0;
        if (_currentUser.UserId.HasValue && _currentUser.Role == "Customer")
        {
            var order = await _orderService.GetInCartOrderAsync(_currentUser.UserId.Value);
            if (order != null)
                count = order.OrderItems.Sum(x => x.Count);
        }

        return View(count);
    }
}