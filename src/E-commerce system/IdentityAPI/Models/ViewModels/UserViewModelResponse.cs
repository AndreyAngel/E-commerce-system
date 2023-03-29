using IdentityAPI.Models.DataBase.Entities;

namespace IdentityAPI.Models.ViewModels;

public class UserViewModelResponse
{
    public string Id { get; set; }

    public string Email { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public DateOnly? BirthDate { get; set; }

    public Address? Address { get; set; }
}
