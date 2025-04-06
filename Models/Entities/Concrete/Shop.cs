using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.Entities.Concrete;

public class Shop : BaseEntity
{ 

    [Required]
    public Guid SellerId { get; set; }

    [ForeignKey("SellerId")]
    public Seller Seller { get; set; } = null!;
 
    [JsonIgnore]
    public ICollection<Product> Products { get; set; } = [];

    [JsonIgnore]
    public ICollection<Order> Orders { get; set; } = [];

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [StringLength(500)]
    public string ShopAddress { get; set; } = null!;

    [Required]
    public string TaxNumber { get; set; } = null!;


    [Required]
    public ShopStatus Status { get; set; }

    public enum ShopStatus
    {
        Approved = 1,
        Closed=2
    }
}
