using AutoDealerPro.Modules.Auth.Core.Entities;

namespace AutoDealerPro.Modules.Auth.Core.Interface
{
    public interface IJwtTokenGenerator
    {
        string? GenerateToken(User? user);
    }
}