﻿using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Models.DTO.Requests;

/// <summary>
/// Registration data transfer object
/// </summary>
public class RegisterDTORequest
{
    /// <summary>
    /// Email
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Incorrect Email")]
    public string? Email { get; set; }

    /// <summary>
    /// BirthDate
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    /// <summary>
    /// Password confirm
    /// </summary>
    [Required]
    [Compare("Password")]
    public string? PasswordConfirm { get; set; }

    /// <summary>
    /// User role
    /// </summary>
    [Required]
    [Range(0, 2, ErrorMessage = "Incorrect role")]
    public RoleDTO Role { get; set; }
}
