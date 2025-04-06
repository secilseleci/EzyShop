namespace Models.ViewModels.Order
{
    public class OrderItemViewModel
    {
        public Guid ProductId { get; set; }   
        public string ProductName { get; set; }   
        public string Color { get; set; }   
        public string ImageUrl { get; set; }   
        public decimal ProductPrice { get; set; }   
        public int Count { get; set; }   
        public decimal TotalPrice => ProductPrice * Count;   
        public Guid ShopId { get; set; }   
        public string ShopName { get; set; }   
    }
}
