using AutoMapper;
using Business.Services.Abstract;
using Core.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models.Identity;

namespace Business.Services.Concrete;

public class AuthService : BaseService, IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    public AuthService(
    UserManager<AppUser> userManager,
    IMapper mapper,
    IConfiguration config,
    ICurrentUserService currentUser)
    : base(mapper, config, currentUser)
    {
        _userManager = userManager;
    }
    public async Task<AppUser?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<bool> CheckPasswordAsync(AppUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }
}
