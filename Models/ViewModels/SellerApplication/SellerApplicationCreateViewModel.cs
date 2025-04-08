using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels.SellerApplication;

public class SellerApplicationCreateViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name can be at most 50 characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, ErrorMessage = "Last name can be at most 50 characters.")]
    public string Surname { get; set; } = null!;

    [Required(ErrorMessage = "Shop name is required.")]
    [StringLength(100, ErrorMessage = "Shop name can be at most 100 characters.")]
    public string ShopName { get; set; } = null!;

    [Required(ErrorMessage = "Shop address is required.")]
    [StringLength(500, ErrorMessage = "Address can be at most 500 characters.")]
    public string ShopAddress { get; set; } = null!;

    [Required(ErrorMessage = "Business phone number is required.")]
    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [Display(Name = "Business Phone")]
    public string ContactBusinessNumber { get; set; } = null!;

    [Required(ErrorMessage = "Tax number is required.")]
    [StringLength(20, ErrorMessage = "Tax number can be at most 20 characters.")]
    public string TaxNumber { get; set; } = null!;
}
