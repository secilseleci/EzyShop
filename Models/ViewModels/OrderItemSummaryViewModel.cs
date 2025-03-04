namespace Models.ViewModels
{
    public class OrderItemSummaryViewModel
    {
        public Guid ProductId { get; set; }  // 🛒 Ürün ID
        public string ProductName { get; set; }  // 🏷 Ürün İsmi
        public string Color { get; set; }  // 🎨 Renk
        public string ImageUrl { get; set; }  // 🖼 Ürün Resmi
        public decimal ProductPrice { get; set; }  // 💲 Fiyat
        public int Count { get; set; }  // 🔢 Adet
        public decimal TotalPrice => ProductPrice * Count;  // 🛍 Toplam Fiyat
    }
}
