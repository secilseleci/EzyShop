
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
 

namespace Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
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

 
    }
}
