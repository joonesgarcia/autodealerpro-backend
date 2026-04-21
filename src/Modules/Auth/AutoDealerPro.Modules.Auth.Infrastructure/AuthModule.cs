using AutoDealerPro.Modules.Auth.Application.Interface;
using AutoDealerPro.Modules.Auth.Application.Services;
using AutoDealerPro.Modules.Auth.Core.Repositories;
using AutoDealerPro.Modules.Auth.Infrastructure.Persistence;
using AutoDealerPro.Modules.Auth.Infrastructure.Util;
using AutoDealerPro.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace AutoDealerPro.Modules.Auth.Infrastructure;

public class AuthModule : IModule
{
    public string Name => "Authentication & Authorization";

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapAuthEndpoints();
    }

    public void Register(IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        services.AddSingleton<JwtTokenGenerator>();
        services.AddSingleton<IAuthService, AuthService>();
    }
}
