using Models.Entities.Abstract;
using Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.Entities.Concrete
{
    public class Shop : IBaseEntity
    {
        public Shop()
        {
            Id = Guid.NewGuid();
            Products = new List<Product>();
          
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

     

        [Required]
        public Guid SellerId { get; set; }

        [ForeignKey("SellerId")]
        public AppUser Seller { get; set; }

        [Required]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Invalid phone number format.")]
        public string BusinessPhoneNumber { get; set; }  
        
        [Required]
        [StringLength(500)]
        public string Address { get; set; }  

        public string? TaxNumber { get; set; }  

        public bool IsActive { get; set; } = true;  

       

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }

        [Required]
        public ShopStatus Status { get; set; }

        public enum ShopStatus
        {
            
            Approved = 1,
            Suspended = 2
        }
    }
}
