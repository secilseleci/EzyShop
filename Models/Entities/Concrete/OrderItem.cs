using System.ComponentModel.DataAnnotations;

namespace Models.Entities.Concrete;

public class OrderItem : BaseEntity
{
     
    [Required]
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;


    [Required]
    public Guid ProductId { get; set; }
    public Product Product { get; set; }=null!;
    
    [Required]
    public string ProductName { get; set; } = null!;

    [Required]
    public decimal ProductPrice { get; set; }
    public decimal TotalPrice => Count * ProductPrice;

    public string? Color { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;

    [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
    public int Count { get; set; } = 1;   
}