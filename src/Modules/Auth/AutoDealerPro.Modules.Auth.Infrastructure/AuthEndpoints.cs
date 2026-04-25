using AutoDealerPro.Modules.Auth.Application.Interface;
using AutoDealerPro.Modules.Auth.Core.Requests;
using AutoDealerPro.Modules.Auth.Core.ResultObjects.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoDealerPro.Modules.Auth.Infrastructure;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/login", async (LoginRequest request, IAuthService service, IValidator<LoginRequest> validator) =>
        {
            var validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);

            var loginResult = await service.HandleLogin(request);

            return loginResult.Status == LoginStatus.Success ?
                Results.Ok(new { token = loginResult.Token }) :
                Results.Unauthorized();
        })
        .AllowAnonymous()
        .WithName("Login");

        group.MapPost("/register", async (CreateAccountRequest request, IAuthService service, IValidator<CreateAccountRequest> validator) =>
        {
            var validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);

            var accountCreationResult = await service.HandleCreateAccount(request);

            return accountCreationResult.Created ?
                Results.Created() :
                Results.BadRequest($"Error: {accountCreationResult.AccountCreationStatus}");
        })
        .AllowAnonymous()
        .WithName("Register");
    }
}
