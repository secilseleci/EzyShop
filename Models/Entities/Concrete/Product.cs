using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities.Concrete;

public class Product : BaseEntity
{
    [ForeignKey(nameof(Category)), Required]
    public Guid CategoryId { get; set; }

    [ForeignKey(nameof(Shop)), Required]
    public Guid ShopId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int Stock { get; set; } = 1;
 
    [Required]
    public bool IsSoldOut => Stock <= 0;

    public string? ImageUrl { get; set; }

    public string? Color { get; set; } 

    public ICollection<OrderItem> OrderItems { get; set; } = [];
}
