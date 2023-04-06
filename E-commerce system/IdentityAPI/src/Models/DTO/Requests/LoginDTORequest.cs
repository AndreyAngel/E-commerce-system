using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Models.DTO;

/// <summary>
/// Login view model
/// </summary>
public class LoginDTORequest
{
    /// <summary>
    /// Gets or sets a Email
    /// </summary>
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets a password
    /// </summary>
    [Required]
    public string? Password { get; set; }
}
