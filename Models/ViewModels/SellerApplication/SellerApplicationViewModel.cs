using Models.Entities.Concrete;

namespace Models.ViewModels.SellerApplication;

public class SellerApplicationViewModel
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = "N/A";
    public string Email { get; set; } = null!;
    public string ContactBusinessNumber { get; set; } = null!;
    public string ShopName { get; set; } = null!;

    public string ShopAddress { get; set; } = null!;
    public string TaxNumber { get; set; } = null!;


    public ApplicationStatus Status { get; set; }
    public string StatusText => Status.ToString();
}
