using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Models.DataBase.Entities;

/// <summary>
/// Entity storing user data
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// Gets or sets a name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets a surname
    /// </summary>
    public string? Surname { get; set; }

    /// <summary>
    /// Gets or sets a birth date
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Gets or sets a address Id
    /// </summary>
    public Guid? AddressId { get; set; }

    /// <summary>
    /// Gets or sets a address
    /// </summary>
    public Address? Address { get; set; }

    /// <summary>
    /// Gets a registration date
    /// </summary>
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
}
