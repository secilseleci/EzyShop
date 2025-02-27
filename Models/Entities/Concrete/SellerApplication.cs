using Models.Entities.Abstract;
using Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities.Concrete
{
    public class SellerApplication:IBaseEntity
    {
        public SellerApplication()
        {
            Id = Guid.NewGuid();
            Status = ApplicationStatus.Pending;
        }
        [Key]
        public Guid Id { get; set; }

        //kişisel veri
         
        public Guid? UserId { get; set; }


        [ForeignKey("UserId")]
        public AppUser? User { get; set; }

        
        [Required]
        public string Email { get; set; }


        [Required]
        public string Name { get; set; }


        [Required]
        public string ContactNumber { get; set; }


        [Required]
        public string Address { get; set; }


        //işyeri ile ilgili veri

        [Required]
        public string StoreName { get; set; }

        public string? TaxNumber { get; set; }
        [Required]
        public string ContactBusinessNumber { get; set; }
        public Guid? ShopId { get; set; }

        public Shop? Shop { get; set; }

        [Required]
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

        [Required]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;  
    }
        public enum ApplicationStatus
        {
            Pending,  
            Approved,  
            Rejected  
        }
}
