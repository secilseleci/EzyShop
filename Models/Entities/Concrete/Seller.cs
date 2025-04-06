using Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.Entities.Concrete;

public class Seller : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public AppUser User { get; set; } = null!;


    [Required]
    public Guid SellerApplicationId { get; set; }
   
    [ForeignKey(nameof(SellerApplicationId))]
    public SellerApplication SellerApplication { get; set; } = null!;

 

    [JsonIgnore]
    public Shop? Shop { get; set; }

}
