namespace Models.ViewModels
{
    public class ShopOrderSummaryViewModel
    {
        public Guid ShopId { get; set; } // 🏪 Mağaza ID
        public string ShopName { get; set; }  // 🏪 Mağaza İsmi
        public decimal TotalAmount => OrderItems.Sum(i => i.TotalPrice);  // 💰 Mağazaya özel toplam tutar
        public List<OrderItemSummaryViewModel> OrderItems { get; set; }  // 🛍 Ürünler
    }
}
