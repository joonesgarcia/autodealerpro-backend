using AutoDealerPro.Modules.Auth.Core.Entities;
using AutoDealerPro.Modules.Auth.Core.Requests;
using AutoDealerPro.Modules.Auth.Core.ResultObjects.Enums;

namespace AutoDealerPro.Modules.Auth.Core.Repositories;

public interface IUserRepository
{
    Task<User?> GetBy(string username);
    Task CreateAccount(User user);
    Task<AccountCreationValidationStatus> ValidateAccountCreation(CreateAccountRequest createAccountRequest);
}
