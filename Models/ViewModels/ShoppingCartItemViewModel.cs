 
namespace Models.ViewModels
{
    public class ShoppingCartItemViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }  
        public string ImageUrl { get; set; }
        public string Color { get; set; }

        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice => Price * Count;   
    }
}