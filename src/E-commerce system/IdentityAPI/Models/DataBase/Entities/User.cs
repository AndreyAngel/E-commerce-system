using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Models.DataBase.Entities;

public class User : IdentityUser
{
    public string? Name { get; set; }

    public string? Surname { get; set; }

    public DateTime? BirthDate { get; set; }

    public Guid? AddressId { get; set; }

    public Address? Address { get; set; }

    public DateTime RegistrationDate { get; set; } = DateTime.Now;
}
