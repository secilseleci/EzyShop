using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities.Concrete;

public class CartLine : BaseEntity
{ 
   
    [Required]
    public Guid CartId { get; set; }

    [ForeignKey("CartId")]
    public Cart Cart { get; set; } = null!;

    [Required]
    public Guid ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; } = null!;

    [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
    public int Count { get; set; } = 1;

    
}
