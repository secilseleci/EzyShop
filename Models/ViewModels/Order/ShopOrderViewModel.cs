namespace Models.ViewModels.Order
{
    public class ShopOrderViewModel
    {
        public Guid ShopId { get; set; } 
        public string ShopName { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal TotalAmount => OrderItems.Sum(i => i.TotalPrice);   
        public List<OrderItemViewModel> OrderItems { get; set; }   
    }
}
