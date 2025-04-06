using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels.User
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
        [RegularExpression(@"^[A-Za-zÇçĞğİıÖöŞşÜü\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Email Address")]
        [EmailAddress]
        [RegularExpression(@".*\.com$", ErrorMessage = "Email must end with .com")]
        public string Email { get; set; }


        [Required]
        public string Address { get; set; }


        [Required]
        public string ContactNumber { get; set; }

        public string Role { get; set; }
        public string? StoreName { get; set; }
        public string? TaxNumber { get; set; }
    }
}
