using AutoDealerPro.Modules.Auth.Application.Interface;
using AutoDealerPro.Modules.Auth.Core.Entities;
using AutoDealerPro.Modules.Auth.Core.Repositories;
using AutoDealerPro.Modules.Auth.Core.Requests;
using AutoDealerPro.Modules.Auth.Core.ResultObjects;
using AutoDealerPro.Modules.Auth.Core.ResultObjects.Enums;
using AutoDealerPro.Modules.Auth.Infrastructure.Util;
using Microsoft.AspNetCore.Identity;
namespace AutoDealerPro.Modules.Auth.Application.Services;

public class AuthService(IUserRepository userRepository, JwtTokenGenerator jwtTokenGenerator) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly JwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

    public async Task<CreateAccountResult> HandleCreateAccount(CreateAccountRequest createAccountRequest)
    {
        var validationResult = await _userRepository.ValidateAccountCreation(createAccountRequest);

        switch (validationResult)
        {
            case AccountCreationValidationStatus.UsernameTaken:
                return new CreateAccountResult(false, validationResult);
            case AccountCreationValidationStatus.EmailTaken:
                return new CreateAccountResult(false, validationResult);
        }

        var user = new User()
        {
            Id = Guid.NewGuid(),
            Email = createAccountRequest.Email,
            Username = createAccountRequest.Username,
            EmailConfirmed = false,
            Roles = ["User"]
        };

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, createAccountRequest.Password);

        await _userRepository.CreateAccount(user);

        return new CreateAccountResult(true, validationResult);
    }

    public async Task<LoginResult> HandleLogin(LoginRequest loginRequest)
    {
        var user = await _userRepository.GetBy(loginRequest.Username);
        if (user == null) return new LoginResult(LoginStatus.InvalidCredentials, null);

        var hasher = new PasswordHasher<User>();

        var loginPasswordHash = hasher.HashPassword(user, loginRequest.Password);
        var verificationResult = hasher.VerifyHashedPassword(user, user.PasswordHash, loginPasswordHash);

        if (verificationResult is not PasswordVerificationResult.Success)
            return new LoginResult(LoginStatus.InvalidCredentials, null);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return await Task.FromResult(new LoginResult(LoginStatus.Success, token));

    }

}
