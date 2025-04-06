using Models.ViewModels.Abstract;

namespace Models.ViewModels.Product;

public class ProductCustomerViewModel : IImageViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? Color { get; set; }
    public string? ImageUrl { get; set; }

 
    public string FolderName { get; set; } = "product";
}
