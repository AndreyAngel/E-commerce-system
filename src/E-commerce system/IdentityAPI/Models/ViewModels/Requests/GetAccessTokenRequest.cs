using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Models.ViewModels.Requests;

/// <summary>
/// Model of request for get access token
/// </summary>
public class GetAccessTokenRequest
{
    /// <summary>
    /// Gets or sets a refresh token
    /// </summary>
    [Required]
    public string? RefreshToken { get; set; }
}
