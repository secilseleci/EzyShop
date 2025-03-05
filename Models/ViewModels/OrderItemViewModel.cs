namespace Models.ViewModels
{
    public class OrderItemViewModel
    {
        public Guid ProductId { get; set; }  // 🛒 Ürün ID
        public string ProductName { get; set; }  // 🏷 Ürün İsmi
        public string Color { get; set; }  // 🎨 Renk
        public string ImageUrl { get; set; }  // 🖼 Ürün Resmi
        public decimal ProductPrice { get; set; }  // 💲 Fiyat
        public int Count { get; set; }  // 🔢 Adet
        public decimal TotalPrice => ProductPrice * Count;  // 🛍 Toplam Fiyat
        public Guid ShopId { get; set; }  // 🏪 Mağaza ID
        public string ShopName { get; set; }  // 🏪 Mağaza İsmi
    }
    }
