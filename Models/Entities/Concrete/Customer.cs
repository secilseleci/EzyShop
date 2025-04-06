using Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.Entities.Concrete;

public class Customer: BaseEntity

{
    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public AppUser User { get; set; } = null!;

    [Required]
    public string ShippingAddress { get; set; } = null!;

    [JsonIgnore]
    public ICollection<Order> Orders { get; set; } = [];

    [JsonIgnore]
    public ShoppingCart? ShoppingCart { get; set; }
}
