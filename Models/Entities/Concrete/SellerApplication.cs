using Models.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities.Concrete
{
    public class SellerApplication:IBaseEntity
    {
        public SellerApplication()
        {
            Id = Guid.NewGuid();
            Status = ApplicationStatus.Pending;
        }
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string StoreName { get; set; }

        public string? TaxNumber { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        public string Address { get; set; }

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
