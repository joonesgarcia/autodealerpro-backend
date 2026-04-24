using AutoDealerPro.Modules.Auth.Core.Entities;
using AutoDealerPro.Modules.Auth.Core.Repositories;
using AutoDealerPro.Modules.Auth.Core.Requests;
using AutoDealerPro.Modules.Auth.Core.ResultObjects;
using System.Security.Cryptography;
using System.Text;

namespace AutoDealerPro.Modules.Auth.Infrastructure.Persistence;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users =
    [
        new User { 
            Id = Guid.NewGuid(), 
            Username = "admin", 
            PasswordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes("strong"))), 
            Roles = ["Admin", "Staff"] 
        },
        new User { 
            Id = Guid.NewGuid(), 
            Username = "staff",
            PasswordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes("another-strong-password"))),
            Roles = ["Staff"] 
        }
    ];

    public async Task<User?> GetBy(string username) =>
        await Task.FromResult(_users.FirstOrDefault(u => u.Username == username));

    public async Task<AccountCreationValidationStatus> ValidateAccountCreation(CreateAccountRequest createAccountRequest)
    {
        var validEmail = await IsValidEmail(createAccountRequest.Email);
        if (!validEmail) return await Task.FromResult(AccountCreationValidationStatus.InvalidEmail);

        var validUsername = await IsValidUserName(createAccountRequest.Username);
        if (!validUsername) return await Task.FromResult(AccountCreationValidationStatus.InvalidUsername);

        return await Task.FromResult(AccountCreationValidationStatus.Valid);
    }
    public Task CreateAccount(CreateAccountRequest createAccountRequest, string passwordHash)
    {
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Email = createAccountRequest.Email,
            Username = createAccountRequest.Username,
            PasswordHash = passwordHash,
            EmailConfirmed = false,
            Roles = ["User"]
        };

        _users.Add(user);

        return Task.CompletedTask;
    }


    private async Task<bool> IsValidUserName(string username) => await Task.FromResult(!_users.Any(x => x.Username == username));
    private async Task<bool> IsValidEmail(string email) => await Task.FromResult(!_users.Any(x => x.Email == email));


}
