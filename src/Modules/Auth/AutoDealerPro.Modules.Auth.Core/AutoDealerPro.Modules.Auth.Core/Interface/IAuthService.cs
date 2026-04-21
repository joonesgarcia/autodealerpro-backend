using AutoDealerPro.Modules.Auth.Core.Requests;

namespace AutoDealerPro.Modules.Auth.Application.Interface;

public interface IAuthService
{
    Task<string?> HandleLogin(LoginRequest login);
}
