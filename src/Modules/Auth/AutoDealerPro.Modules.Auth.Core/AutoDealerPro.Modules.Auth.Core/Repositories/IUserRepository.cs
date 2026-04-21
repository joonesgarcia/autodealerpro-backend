using AutoDealerPro.Modules.Auth.Core.Entities;

namespace AutoDealerPro.Modules.Auth.Core.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByUsername(string username);
}
