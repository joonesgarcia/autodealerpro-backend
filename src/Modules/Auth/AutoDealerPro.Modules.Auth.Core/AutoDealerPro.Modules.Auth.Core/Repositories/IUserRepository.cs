using AutoDealerPro.Modules.Auth.Core.Entities;
using AutoDealerPro.Modules.Auth.Core.Requests;
using AutoDealerPro.Modules.Auth.Core.ResultObjects;

namespace AutoDealerPro.Modules.Auth.Core.Repositories;

public interface IUserRepository
{
    Task<User?> GetBy(string username);
    Task CreateAccount(CreateAccountRequest createAccountRequest, string passwordHash);
    Task<AccountCreationValidationStatus> ValidateAccountCreation(CreateAccountRequest createAccountRequest);
}
