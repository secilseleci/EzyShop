namespace Models.ViewModels.Customer;

public class CustomerListViewModel
{
    public Guid CustomerId { get; set; }

    public string FullName { get; set; } = "N/A";

    public string Email { get; set; } = "N/A";

    public string PhoneNumber { get; set; } = "-";
}