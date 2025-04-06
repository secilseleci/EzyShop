using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Models.ViewModels.Customer;

public class RegisterViewModel
{
    [Required]
    [DisplayName("Full Name")]
    public string FullName { get; set; } = null!;

    [Required]
    [DisplayName("Email Address")]
    [EmailAddress]
    [RegularExpression(@".*\.com$", ErrorMessage = "The email must end with .com")]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [MinLength(6)]
    [Compare(nameof(Password))]
    [DisplayName("Confirm Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;

    [Required]
    [DisplayName("Contact Number")]
    [Phone]
    [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Enter a valid contact number (10-15 digits)")]
    public string ContactNumber { get; set; } = null!;

    [Required]
    [DisplayName("Address")]
    [StringLength(255, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 255 characters.")]
    public string Address { get; set; } = null!;
}
