using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Models.DTO.Requests;

/// <summary>
/// Stores data for changing password
/// </summary>
public class ChangePasswordDTORequest
{
    /// <summary>
    /// User Email
    /// </summary>
    [Required]
    public string? Email { get; set; }

    /// <summary>
    /// Old password
    /// </summary>
    [Required]
    public string? OldPassword { get; set; }

    /// <summary>
    /// New password
    /// </summary>
    [Required]
    public string? NewPassword { get; set; }
}
