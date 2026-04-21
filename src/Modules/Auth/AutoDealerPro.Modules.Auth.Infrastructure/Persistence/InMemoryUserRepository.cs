using AutoDealerPro.Modules.Auth.Core.Entities;
using AutoDealerPro.Modules.Auth.Core.Repositories;

namespace AutoDealerPro.Modules.Auth.Infrastructure.Persistence;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users =
    [
        new User { Id = Guid.NewGuid(), Username = "admin", Password = "a-example-password", Roles = ["Admin", "Staff"] },
        new User { Id = Guid.NewGuid(), Username = "staff", Password = "a-example-password",  Roles = ["Staff"] }
    ];

    public async Task<User?> GetUserByUsername(string username)
    => await Task.FromResult(_users.FirstOrDefault(u => u.Username == username));
}
