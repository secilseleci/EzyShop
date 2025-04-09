using Models.Entities.Concrete;

namespace Models.ViewModels.Seller;

public class SellerListViewModel
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = "N/A";

    public string Email { get; set; } = "N/A";

    public string PhoneNumber { get; set; } = "-";
    public string? ShopName { get;set; } = "N/A";
    public string StatusText => Status.ToString();

    public ApplicationStatus Status { get; set; }
 
    public string TaxNumber { get; set; } = null!;
}
