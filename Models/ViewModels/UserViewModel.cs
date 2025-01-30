using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class UserViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Email Address")]
        [EmailAddress]
        [RegularExpression(@".*\.com$", ErrorMessage = "Email must end with .com")]
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new();


    }
}
