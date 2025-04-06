using System.ComponentModel.DataAnnotations;
using static Models.Entities.Concrete.Shop;

namespace Models.ViewModels.Shop;

public class ShopViewModel
{
    public Guid Id { get; set; }  

    [Required(ErrorMessage = "Shop name is required.")]
    [StringLength(100, ErrorMessage = "Shop name cannot exceed 50 characters.")]
    public string Name { get; set; } = null!;
   
   
    [Required(ErrorMessage = "Address is required.")]
    [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters.")]
    public string ShopAddress { get; set; } = null!;

    public string TaxNumber { get; set; } = null!;


    public string StatusText => Status.ToString();
    public ShopStatus Status { get; set; }

    public string? SellerName { get; set; }
    
    [Required]
    public Guid SellerId { get; set; }
}
