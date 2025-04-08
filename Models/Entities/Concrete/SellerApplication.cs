using Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities.Concrete;

public class SellerApplication:BaseEntity
{
    public Guid? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public AppUser? User { get; set; }

    public Guid? SellerId { get; set; }

    [ForeignKey(nameof(SellerId))]
    public Seller? Seller { get; set; }


    [Required]
    public  string Email { get; set; } = null!;
    
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Surname { get; set; } = null!;
    [Required]
    public string ShopName { get; set; } = null!;
   
    [Required]
    [StringLength(500)]
    public string ShopAddress { get; set; } = null!;

    [Required]
    public string ContactBusinessNumber { get; set; } = null!;

    [Required]
    public string TaxNumber { get; set; } = null!;


    [Required]
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
}
    public enum ApplicationStatus
        {
            Pending=1,  
            Approved=2,  
            Rejected=3
        }
