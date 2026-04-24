using AutoDealerPro.Modules.Auth.Core.Requests;
using AutoDealerPro.Modules.Auth.Core.ResultObjects;

namespace AutoDealerPro.Modules.Auth.Application.Interface;

public interface IAuthService
{
    Task<LoginResult> HandleLogin(LoginRequest credentials);
    Task<CreateAccountResult> HandleCreateAccount(CreateAccountRequest accountDetails);

}
