using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class UserProfileViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
        [RegularExpression(@"^[A-Za-zÇçĞğİıÖöŞşÜü\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        public string Name { get; set; }

        [Required, ReadOnly(true)]  
        [DisplayName("Email Address")]
        [EmailAddress]
        [RegularExpression(@".*\.com$", ErrorMessage = "Email must end with .com")]
        public string Email { get; set; }


        [Required]
        public string Address { get; set; } 


        [Required]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Phone number must be 10 or 11 digits.")] // ☎️ Numara 10-11 hane olmalı

        public string ContactNumber { get; set; }
 
    }
}
