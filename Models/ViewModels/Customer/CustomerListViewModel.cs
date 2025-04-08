using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels.Customer;

public class CustomerListViewModel
{
    [Required] 
    public Guid Id { get; set; }

    public string FullName { get; set; } = "N/A";

    public string Email { get; set; } = "N/A";

    public string PhoneNumber { get; set; } = "-";
}