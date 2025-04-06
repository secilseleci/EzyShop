using AutoMapper;
using Core.Security;
using Microsoft.Extensions.Configuration;

namespace Business.Services.Concrete;

public abstract class BaseService
{
    protected readonly IMapper Mapper;
    protected readonly IConfiguration Config;
    protected readonly ICurrentUserService CurrentUser;

    protected BaseService(
        IMapper mapper,
        IConfiguration config,
        ICurrentUserService currentUser)
    {
        Mapper = mapper;
        Config = config;
        CurrentUser = currentUser;
    }

    protected Guid? GetUserId() => CurrentUser.UserId;
}
