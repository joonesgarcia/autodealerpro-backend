using AutoDealerPro.Modules.Auth.Core.Entities;
using AutoDealerPro.Modules.Auth.Core.Repositories;
using AutoDealerPro.Modules.Auth.Core.Requests;
using AutoDealerPro.Modules.Auth.Core.ResultObjects.Enums;
using Microsoft.AspNetCore.Identity;

namespace AutoDealerPro.Modules.Auth.Infrastructure.Persistence;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users =
    [
        new User {
            Id = Guid.NewGuid(),
            Username = "theadministrator",
            Roles = ["Admin", "Staff"]
        }
    ];

    public InMemoryUserRepository()
    {
        var mockAdmin = _users.First();
        mockAdmin.PasswordHash = new PasswordHasher<User>().HashPassword(mockAdmin, "astrongpassword");
    }

    public async Task<User?> GetBy(string username) =>
        await Task.FromResult(_users.FirstOrDefault(u => u.Username == username));

    public async Task<AccountCreationValidationStatus> ValidateAccountCreation(CreateAccountRequest createAccountRequest)
    {
        var validEmail = await IsValidEmail(createAccountRequest.Email);
        if (!validEmail) return await Task.FromResult(AccountCreationValidationStatus.EmailTaken);

        var validUsername = await IsValidUserName(createAccountRequest.Username);
        if (!validUsername) return await Task.FromResult(AccountCreationValidationStatus.UsernameTaken);

        return await Task.FromResult(AccountCreationValidationStatus.Valid);
    }
    public Task CreateAccount(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }


    private async Task<bool> IsValidUserName(string username) => await Task.FromResult(!_users.Any(x => x.Username == username));
    private async Task<bool> IsValidEmail(string email) => await Task.FromResult(!_users.Any(x => x.Email == email));


}
