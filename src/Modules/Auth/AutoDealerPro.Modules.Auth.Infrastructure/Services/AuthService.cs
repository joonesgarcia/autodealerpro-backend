using AutoDealerPro.Modules.Auth.Application.Interface;
using AutoDealerPro.Modules.Auth.Core.Repositories;
using AutoDealerPro.Modules.Auth.Core.Requests;
using AutoDealerPro.Modules.Auth.Infrastructure.Util;
namespace AutoDealerPro.Modules.Auth.Application.Services;

public class AuthService(IUserRepository userRepository, JwtTokenGenerator jwtTokenGenerator) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly JwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

    public async Task<string?> HandleLogin(LoginRequest login)
    {
        var user = await _userRepository.GetUserByUsername(login.Username);
        var token = _jwtTokenGenerator.GenerateToken(user);
        return await Task.FromResult(token);
    }
}
