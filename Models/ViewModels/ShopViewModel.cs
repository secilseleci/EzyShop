using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class ShopViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Shop name is required.")]
        [StringLength(100, ErrorMessage = "Shop name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Invalid phone number format.")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters.")]
        public string Address { get; set; }

        public string? TaxNumber { get; set; } 

        public bool IsActive { get; set; } = false; 

        public Guid SellerId { get; set; }

        public string Status { get; set; } = "Pending";  

        public string? SellerName { get; set; }  
    }
}
