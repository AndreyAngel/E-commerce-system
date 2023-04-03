﻿using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Models.ViewModels.Requests;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}