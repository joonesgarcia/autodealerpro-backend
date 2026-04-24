using AutoDealerPro.Modules.Auth.Application.Interface;
using AutoDealerPro.Modules.Auth.Core.Repositories;
using AutoDealerPro.Modules.Auth.Core.Requests;
using AutoDealerPro.Modules.Auth.Core.ResultObjects;
using AutoDealerPro.Modules.Auth.Core.ResultObjects.Enums;
using AutoDealerPro.Modules.Auth.Infrastructure.Util;
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
            case AccountCreationValidationStatus.InvalidUsername:
                return new CreateAccountResult(false, validationResult);
            case AccountCreationValidationStatus.InvalidEmail:
                return new CreateAccountResult(false, validationResult);
            case AccountCreationValidationStatus.WeakPassword:
                return new CreateAccountResult(false, validationResult);
        }

        var passwordHash = PasswordHasher.Hash(createAccountRequest.Password);
        await _userRepository.CreateAccount(createAccountRequest, passwordHash);
        
        return new CreateAccountResult(true, validationResult); 
    }

    public async Task<LoginResult> HandleLogin(LoginRequest loginRequest)
    {
        var user = await _userRepository.GetBy(loginRequest.Username);
        if (user == null) return new LoginResult(LoginStatus.InvalidCredentials, null);

        var validPassword = PasswordHasher.Verify(loginRequest.Password, user.PasswordHash);
        if (!validPassword) return new LoginResult(LoginStatus.InvalidCredentials, null);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return await Task.FromResult(new LoginResult(LoginStatus.Success, token));

    }

}
