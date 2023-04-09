using IdentityAPI.Models.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityAPI.Helpers;

/// <summary>
/// Class for working with JWT tokens
/// </summary>
public static class JwtTokenHelper
{
    /// <summary>
    /// Generate of the JWT refresh token
    /// </summary>
    /// <param name="configuration"> Configurations of application </param>
    /// <param name="claims"> User claims </param>
    /// <returns> A string containing the JWT refresh token </returns>
    public static string GenerateJwtRefreshToken(IConfiguration configuration, List<Claim> claims)
    {
        return GenerateJwtToken(configuration, claims, TokenType.Refresh);
    }

    /// <summary>
    /// Generate of the JWT access token
    /// </summary>
    /// <param name="configuration"> Configurations of application </param>
    /// <param name="claims"> User claims </param>
    /// <returns> A string containing the JWT access token </returns>
    public static string GenerateJwtAccessToken(IConfiguration configuration, List<Claim> claims)
    {
        return GenerateJwtToken(configuration, claims, TokenType.Access);
    }

    /// <summary>
    /// Validate of token
    /// </summary>
    /// <param name="configuration"> Configurations of application </param>
    /// <param name="token"> Token to verify </param>
    /// <returns> Validated token </returns>
    public static string ValidateToken(IConfiguration configuration, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Authentication:Secret"]);

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidIssuer = configuration["Authentication:Issuer"],
            ValidateAudience = false,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        }, out SecurityToken validatedToken);

        return tokenHandler.WriteToken((JwtSecurityToken)validatedToken);
    }

    /// <summary>
    /// Generate JWT token
    /// </summary>
    /// <param name="configuration"> Configurations of application </param>
    /// <param name="claims"> User claims </param>
    /// <param name="tokenType"> Token type </param>
    /// <returns> JWT token </returns>
    private static string GenerateJwtToken(IConfiguration configuration, List<Claim> claims, TokenType tokenType)
    {
        var key = Encoding.UTF8.GetBytes(configuration["Authentication:Secret"]);

        DateTime expirationTime;

        if (tokenType == TokenType.Refresh)
        {
            expirationTime = DateTime.UtcNow.AddMonths(3);
        }

        else
        {
            expirationTime = DateTime.UtcNow.AddMinutes(15);
        }

        var token = new JwtSecurityToken(
            configuration["Authentication:Issuer"],
            null,
            claims,
            DateTime.UtcNow,
            expirationTime,
            new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
