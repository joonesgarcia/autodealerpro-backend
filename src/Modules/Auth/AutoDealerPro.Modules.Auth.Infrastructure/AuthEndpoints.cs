using AutoDealerPro.Modules.Auth.Application.Interface;
using AutoDealerPro.Modules.Auth.Core.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoDealerPro.Modules.Auth.Infrastructure;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/login", (LoginRequest req, IAuthService service) =>
        {
            var token = service.HandleLogin(req);
            return token is not null ? Results.Ok(new { token }) : Results.Unauthorized();
        })
        .AllowAnonymous()
        .WithName("Login");
    }
}
