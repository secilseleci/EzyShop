using Models.ViewModels.Abstract;

namespace Models.ViewModels.Product;

public class ProductSellerViewModel : IImageViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? Color { get; set; }

    public bool IsActive { get; set; }
    public string StatusText => IsActive ? "Active" : "Passive";
    public string StockStatus => Stock <= 0 ? "Out of Stock" : "In Stock";

    public string CategoryName { get; set; } = "N/A";

    public string? ImageUrl { get; set; }
    public string FolderName { get; set; } = "product";
}

