using AutoDealerPro.Modules.Auth.Core.Requests.V1.CreateAccount;
using AutoDealerPro.Modules.Auth.Core.Requests.V1.Login;
using AutoDealerPro.Modules.Auth.Core.Result;

namespace AutoDealerPro.Modules.Auth.Core.Interface;

public interface IAuthService
{
    Task<LoginResult> HandleLogin(LoginRequestV1 credentials);
    Task<CreateAccountResult> HandleCreateAccount(CreateAccountRequestV1 accountDetails);

}
