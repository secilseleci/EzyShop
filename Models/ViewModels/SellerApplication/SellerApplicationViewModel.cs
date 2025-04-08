using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Models.Entities.Concrete;

namespace Models.ViewModels.SellerApplication;

public class SellerApplicationViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [DisplayName("E-mail")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Name is required.")]
    [MinLength(5, ErrorMessage = "Name must be at least 5 characters.")]
    [DisplayName("Full Name")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Store Name is required.")]
    [MaxLength(30, ErrorMessage = "Store Name cannot exceed 30 characters.")]
    [DisplayName("Store Name")]
    public string StoreName { get; set; } = null!;

    [Required(ErrorMessage = "Phone Number is required.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be exactly 11 digits.")]
    [DisplayName("Phone Number")]
    public string ContactBusinessNumber { get; set; } = null!;

    [Required]
    [DisplayName("Tax Number")]
    public string TaxNumber { get; set; } = null!;

    public string StatusText => Status.ToString();


    // Sadece admin veya sistem içi işler için
    public Guid? UserId { get; set; }
    public Guid? SellerId { get; set; }
     
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

 }
