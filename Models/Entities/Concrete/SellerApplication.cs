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
            IsDeleted = false;
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


        //iş yeri ile ilgili veri

        [Required]
        public string StoreName { get; set; }

        [Required]
        public string ContactBusinessNumber { get; set; }


        public string? TaxNumber { get; set; }
        
        public bool IsDeleted { get; set; }


        [Required]
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

        [Required]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    }
        public enum ApplicationStatus
            {
                Pending=1,  
                Approved=2,  
                Rejected =3
            }
}
