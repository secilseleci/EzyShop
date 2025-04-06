using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities.Concrete;

public class ShoppingCartItem : BaseEntity
{ 
   
    [Required]
    public Guid CartId { get; set; }

    [ForeignKey("CartId")]
    public ShoppingCart Cart { get; set; } = null!;

    [Required]
    public Guid ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; } = null!;

    [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
    public int Count { get; set; } = 1;

    [Required]
    public decimal Price { get; set; }
}
