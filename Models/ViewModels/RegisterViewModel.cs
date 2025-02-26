using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [Required]
        [DisplayName("Email Address")]
        [EmailAddress]
        [RegularExpression(@".*\.com$", ErrorMessage = "The email must end with .com")]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(6)]
        [Compare(nameof(Password))]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [DisplayName("Contact Number")]
        [Phone]
        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Enter a valid contact number (10-15 digits)")]
        public string ContactNumber { get; set; }

        [Required]
        [DisplayName("Address")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 255 characters.")]
        public string Address { get; set; }
    }
}
