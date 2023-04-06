using IdentityAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Models.DTO;

/// <summary>
/// Registration view model
/// </summary>
public class RegisterDTORequest
{
    /// <summary>
    /// Gets or sets a Email
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Incorrect Email")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets a BirthDate
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Gets or sets a password
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets a password confirm
    /// </summary>
    [Compare("Password")]
    public string? PasswordConfirm { get; set; }

    /// <summary>
    /// Gets or sets a role
    /// </summary>
    [Range(0, 3, ErrorMessage = "Incorrect role")]
    public Role Role { get; set; }
}
