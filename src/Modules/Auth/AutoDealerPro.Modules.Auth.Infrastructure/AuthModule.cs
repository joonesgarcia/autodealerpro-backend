using AutoDealerPro.Modules.Auth.Application.Interface;
using AutoDealerPro.Modules.Auth.Application.Services;
using AutoDealerPro.Modules.Auth.Core.Entities;
using AutoDealerPro.Modules.Auth.Core.Interface;
using AutoDealerPro.Modules.Auth.Core.Repositories;
using AutoDealerPro.Modules.Auth.Core.Requests;
using AutoDealerPro.Modules.Auth.Core.Validators;
using AutoDealerPro.Modules.Auth.Infrastructure.Persistence;
using AutoDealerPro.Modules.Auth.Infrastructure.Util;
using AutoDealerPro.Shared.Abstractions.Modules;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutoDealerPro.Modules.Auth.Infrastructure;

public class AuthModule : IModule
{
    public string Name => "Authentication & Authorization";

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapAuthEndpoints();
    }

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        services.AddScoped<IJwtTokenGenerator,JwtTokenGenerator>();

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IValidator<CreateAccountRequest>, CreateAccountValidator>();
        services.AddScoped<IValidator<LoginRequest>, LoginValidator>();
    }
}
