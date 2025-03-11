using Models.Entities.Abstract;
using Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models.Entities.Concrete
{
    public class Shop : IBaseEntity
    {
        public Shop()
        {
            Id = Guid.NewGuid();
            Products = new List<Product>();
            Status = "Pending";  
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

     

        [Required]
        public Guid SellerId { get; set; }

        public AppUser Seller { get; set; }

        [Required]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Invalid phone number format.")]
        public string ContactNumber { get; set; }  
        [Required]
        [StringLength(500)]
        public string Address { get; set; }  

        public string? TaxNumber { get; set; }  

        public bool IsActive { get; set; } = false;  

        [Required]
        [StringLength(10)]
        public string Status { get; set; }
        [JsonIgnore]
         public ICollection<Product> Products { get; set; }
        public SellerApplication? SellerApplication { get; set; }


    }
}
