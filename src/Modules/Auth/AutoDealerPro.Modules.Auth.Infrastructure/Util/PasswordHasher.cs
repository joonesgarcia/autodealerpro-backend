namespace AutoDealerPro.Modules.Auth.Infrastructure.Util;

public static class PasswordHasher
{
    public static string Hash(string password) => Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(password)));
    public static bool Verify(string password, string hash) => Hash(password) == hash;
}
