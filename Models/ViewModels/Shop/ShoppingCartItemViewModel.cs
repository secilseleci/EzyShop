namespace Models.ViewModels.Shop
{
    public class ShoppingCartItemViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public string Color { get; set; }

        public decimal Price { get; set; }

        public Guid ShopId { get; set; }
        public string ShopName { get; set; }
        public int Count { get; set; } = 1;
        public decimal TotalPrice => Price * Count;
    }
}