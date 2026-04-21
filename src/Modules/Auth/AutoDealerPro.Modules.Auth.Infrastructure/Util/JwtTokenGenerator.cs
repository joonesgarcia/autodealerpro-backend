using AutoDealerPro.Modules.Auth.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace AutoDealerPro.Modules.Auth.Infrastructure.Util;

public class JwtTokenGenerator(IConfiguration config)
{
    private readonly IConfiguration _config = config;

    public string? GenerateToken(User? user)
    {
        if (user == null) return null;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username)
        };
        claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["ApiSettings:Secret"] ?? ""));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _config["ApiSettings:Issuer"],
            audience: _config["ApiSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
