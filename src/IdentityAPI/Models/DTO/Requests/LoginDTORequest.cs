using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Models.DTO.Requests;

/// <summary>
/// Login data transfer object
/// </summary>
public class LoginDTORequest
{
    /// <summary>
    /// Email
    /// </summary>
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    [Required]
    public string? Password { get; set; }
}
