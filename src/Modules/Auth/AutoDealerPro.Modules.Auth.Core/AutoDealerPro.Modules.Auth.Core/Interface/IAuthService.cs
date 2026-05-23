using AutoDealerPro.Modules.Auth.Core.Requests.CreateAccount;
using AutoDealerPro.Modules.Auth.Core.Requests.Login;
using AutoDealerPro.Modules.Auth.Core.Result;

namespace AutoDealerPro.Modules.Auth.Core.Interface;

public interface IAuthService
{
    Task<LoginResult> HandleLogin(LoginRequest credentials);
    Task<CreateAccountResult> HandleCreateAccount(CreateAccountRequest accountDetails);

}
