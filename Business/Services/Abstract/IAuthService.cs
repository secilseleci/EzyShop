using Models.Identity;

namespace Business.Services.Abstract;

public interface IAuthService
{
    Task<AppUser?> FindByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(AppUser user, string password);
}
