using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Models.DTO;

/// <summary>
/// Model of request for get access token
/// </summary>
public class GetAccessTokenDTORequest
{
    /// <summary>
    /// Refresh token
    /// </summary>
    [Required]
    public string? RefreshToken { get; set; }
}
