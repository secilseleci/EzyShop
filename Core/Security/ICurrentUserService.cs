namespace Core.Security;

public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Email { get; }
        string? UserName { get; }
        bool IsAuthenticated { get; }
    }
 
