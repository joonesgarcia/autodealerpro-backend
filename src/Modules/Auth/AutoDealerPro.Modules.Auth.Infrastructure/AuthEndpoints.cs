using AutoDealerPro.Modules.Auth.Core.Interface;
using AutoDealerPro.Modules.Auth.Core.Requests.V1.CreateAccount;
using AutoDealerPro.Modules.Auth.Core.Requests.V1.Login;
using AutoDealerPro.Modules.Auth.Core.Responses.V1.CreateAccount;
using AutoDealerPro.Modules.Auth.Core.Result;
using AutoDealerPro.Modules.Auth.Core.Result.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AutoDealerPro.Modules.Auth.Infrastructure;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder group = endpoints.MapGroup("/api/v1/auth").WithTags("Auth");

        group.MapPost("/login", async (LoginRequestV1 request, IAuthService service, IValidator<LoginRequestV1> validator) =>
        {
            FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);

            LoginResult loginResult = await service.HandleLogin(request);

            return loginResult.Status == LoginStatus.Success ?
                Results.Ok(loginResult.Token) :
                Results.Unauthorized();
        })
        .AllowAnonymous()
        .WithName("Login")
        .Produces(401)
        .Produces<string>(201);

        group.MapPost("/register", async (CreateAccountRequestV1 request, IAuthService service, IValidator<CreateAccountRequestV1> validator) =>
        {
            FluentValidation.Results.ValidationResult validation = await validator.ValidateAsync(request);

            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);

            CreateAccountResult accountCreationResult = await service.HandleCreateAccount(request);

            return accountCreationResult.Created ?
                Results.Created() :
                Results.BadRequest(new CreateAccountResponseV1(accountCreationResult.Created, accountCreationResult.AccountCreationStatus.ToString()));
        })
        .AllowAnonymous()
        .WithName("Register")
         .Produces(201)
        .Produces<CreateAccountResponseV1>(400); ;
    }
}
