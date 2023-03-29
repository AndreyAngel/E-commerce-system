using IdentityAPI.Models.DataBase.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Models.ViewModels;

public class AuthenticateViewModelResponse
{
    public string? Id { get; set; }

    public string? Email { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public DateTime? BirthDate { get; set; }

    public Address? Address { get; set; }

    public string Role { get; set; }

    public string? Token { get; set; }

    public List<IdentityError> Errors { get; set; }

    public AuthenticateViewModelResponse(User user, string token, string role)
    {
        Id = user.Id;
        Email = user.Email;
        Name = user.Name;
        Surname = user.Surname;
        BirthDate = user.BirthDate;
        Address = user.Address;
        Role = role;
        Token = token;
        Errors = new List<IdentityError>();
    }

    public AuthenticateViewModelResponse(List<IdentityError> errors)
    {
        Errors = errors;
    }
}
