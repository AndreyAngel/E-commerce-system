using IdentityAPI.Models.DataBase.Entities;
using IdentityAPI.Models.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityAPI.Helpers;

public static class JwtTokenHelper
{
    public static string GenerateJwtRefreshToken(IConfiguration configuration, List<Claim> claims)
    {
        return GenerateJwtToken(configuration, claims, TypeToken.Refresh);
    }

    public static string GenerateJwtAccessToken(IConfiguration configuration, List<Claim> claims)
    {
        return GenerateJwtToken(configuration, claims, TypeToken.Access);
    }

    public static string ValidateToken(IConfiguration configuration, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Secret"]);

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidIssuer = configuration["Issuer"],
            ValidateAudience = false,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        }, out SecurityToken validatedToken);

        return tokenHandler.WriteToken((JwtSecurityToken)validatedToken);
    }

    private static string GenerateJwtToken(IConfiguration configuration, List<Claim> claims, TypeToken typeToken)
    {
        var key = Encoding.UTF8.GetBytes(configuration["Secret"]);

        DateTime expirationTime;

        if (typeToken == TypeToken.Refresh)
        {
            expirationTime = DateTime.UtcNow.AddMonths(1);
        }

        else
        {
            expirationTime = DateTime.UtcNow.AddMinutes(15);
        }

        var token = new JwtSecurityToken(
            configuration["Issuer"],
            null,
            claims,
            DateTime.UtcNow,
            expirationTime,
            new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
