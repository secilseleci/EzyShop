
namespace Models.ViewModels.Product;

public class ProductDetailViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? Color { get; set; }
    public int Stock { get; set; }
    public string? ImageUrl { get; set; }
    public string CategoryName { get; set; } = null!;
    public string ShopName { get; set; } = null!;
    public bool IsSoldOut => Stock <= 0;
}
