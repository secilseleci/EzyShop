using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Models.Identity;

public class AppUser:IdentityUser<Guid> 
{
    public DateTime? LastLoginDate { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Surname { get; set; } = null!;
    public string FullName => $"{Name} {Surname}".Trim();
}
