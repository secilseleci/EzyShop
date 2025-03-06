using Models.Entities.Concrete;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels.User
{
    public class SellerRegistrationViewModel
    {

        // ✅ AppUser bilgileri
        [Required(ErrorMessage = "Name is required.")]
        [MinLength(5, ErrorMessage = "Name cannot exceed 5 characters.")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact Number is required.")]
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        [Display(Name = "Phone Number")]
        public string ContactNumber { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        // ✅ Shop bilgileri
        [Required(ErrorMessage = "Store Name is required.")]
        [MaxLength(30, ErrorMessage = "Store Name cannot exceed 30 characters.")]
        [Display(Name = "Store Name")]
        public string StoreName { get; set; }


        [Required]
        [Display(Name = "Tax Number")]
        public string? TaxNumber { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [Display(Name = "Business Address")]
        public string BusinessAddress { get; set; }
        public string ContactBusinessNumber { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;


    }
}
