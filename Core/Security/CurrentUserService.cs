using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Core.Security;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CurrentUserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public bool IsAuthenticated =>
        _contextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public Guid? UserId
    {
        get
        {
            var userIdClaim = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdClaim?.Value, out var id) ? id : null;
        }
    }
    public string? Email =>
         _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

    public string? UserName =>
        _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;   
    }
 