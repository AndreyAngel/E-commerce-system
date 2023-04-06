namespace IdentityAPI.Models.DTO;

/// <summary>
/// View model for update user data
/// </summary>
public class UserDTORequest
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
    /// Gets or sets a BirthDate
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Gets or sets a address
    /// </summary>
    public AddressDTO? Address { get; set; }
}
