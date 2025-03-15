using Microsoft.AspNetCore.Identity;
using Models.Entities.Abstract;
using Models.Entities.Concrete;
using System.ComponentModel.DataAnnotations;

namespace Models.Identity
{
    public class AppUser:IdentityUser<Guid>,IBaseEntity
    {
        [Required]
        public string Name { get; set; }
        public bool IsSeller { get; set; } = false;
        public bool IsActive { get; set; }=true;
        public string? StoreName { get; set; }  
        public string? TaxNumber { get; set; }  
        public string Address { get; set; }
        public Shop? Shop { get; set; }
        public SellerApplication? SellerApplication { get; set; }

    }
}
