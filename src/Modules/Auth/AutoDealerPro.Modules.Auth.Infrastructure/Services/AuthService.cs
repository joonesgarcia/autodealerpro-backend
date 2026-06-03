using AutoDealerPro.Modules.Auth.Core.Entities;
using AutoDealerPro.Modules.Auth.Core.Interface;
using AutoDealerPro.Modules.Auth.Core.Repositories;
using AutoDealerPro.Modules.Auth.Core.Requests.V1.CreateAccount;
using AutoDealerPro.Modules.Auth.Core.Requests.V1.Login;
using AutoDealerPro.Modules.Auth.Core.Result;
using AutoDealerPro.Modules.Auth.Core.Result.Enums;
using Microsoft.AspNetCore.Identity;
namespace AutoDealerPro.Modules.Auth.Infrastructure.Services;

public class AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IPasswordHasher<User> passwordHasher) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    public async Task<CreateAccountResult> HandleCreateAccount(CreateAccountRequestV1 createAccountRequest)
    {
        AccountCreationValidationStatus validationResult = await _userRepository.ValidateAccountCreation(createAccountRequest);

        if (validationResult != AccountCreationValidationStatus.Valid)
            return new CreateAccountResult(false, validationResult);

        User user = new User()
        {
            Id = Guid.NewGuid(),
            Email = createAccountRequest.Email,
            Username = createAccountRequest.Username,
            EmailConfirmed = false,
            Roles = ["User"]
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, createAccountRequest.Password);

        await _userRepository.CreateAccount(user);

        return new CreateAccountResult(true, validationResult);
    }

    public async Task<LoginResult> HandleLogin(LoginRequestV1 loginRequest)
    {
        User? user = await _userRepository.GetBy(loginRequest.Username);
        if (user == null) return new LoginResult(LoginStatus.InvalidCredentials, null);

        PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password);

        if (verificationResult is not PasswordVerificationResult.Success)
            return new LoginResult(LoginStatus.InvalidCredentials, null);

        string? token = _jwtTokenGenerator.GenerateToken(user);

        return await Task.FromResult(new LoginResult(LoginStatus.Success, token));

    }

}
