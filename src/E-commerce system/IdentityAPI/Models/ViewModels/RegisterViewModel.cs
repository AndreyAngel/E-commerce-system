using System.ComponentModel.DataAnnotations;
using IdentityAPI.Models.Enums;

namespace IdentityAPI.Models.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Incorrect Email")]
    public string? Email { get; set; }

    public DateTime? BirthDate { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    [Compare("Password")]
    public string? PasswordConfirm { get; set; }

    [Range(0, 3, ErrorMessage = "Incorrect role")]
    public Role Role { get; set; }
}
