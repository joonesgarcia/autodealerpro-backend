using AutoDealerPro.Modules.Auth.Core.Entities;
using AutoDealerPro.Modules.Auth.Core.Requests.V1.CreateAccount;
using AutoDealerPro.Modules.Auth.Core.Result.Enums;

namespace AutoDealerPro.Modules.Auth.Core.Repositories;

public interface IUserRepository
{
    Task<User?> GetBy(string username);
    Task CreateAccount(User user);
    Task<AccountCreationValidationStatus> ValidateAccountCreation(CreateAccountRequestV1 createAccountRequest);
}
