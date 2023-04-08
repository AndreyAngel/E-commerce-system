using IdentityAPI.Models.Enums;

namespace IdentityAPI.DataBase.Entities;

/// <summary>
/// Entity storing token information
/// </summary>
public class Token
{
    /// <summary>
    /// Gets or set a Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or set a Id of user using this token
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Gets or set a token type: Access token or Refresh token
    /// </summary>
    public TokenType TokenType { get; init; }

    /// <summary>
    /// Gets or set a token value
    /// </summary>
    public string Value { get; init; }

    /// <summary>
    /// Gets or set a token status:
    /// true, if active
    /// false, if not active
    /// </summary>
    public bool IsActive { get; set; } = true;
}
