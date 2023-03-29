using IdentityAPI.Models.DataBase.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityAPI.Services;

public static class JwtTokenService
{
    public static string GenerateJwtToken(this IConfiguration configuration, User user, string role)
    {
        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim("Role", role)
        };

        var key = Encoding.UTF8.GetBytes(configuration["Secret"]);

        var token = new JwtSecurityToken(
            configuration["Issuer"],
            null,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(7),
            new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
