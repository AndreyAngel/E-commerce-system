namespace IdentityAPI.Models.DTO;

/// <summary>
/// The view model of the response containing the user data
/// </summary>
public class UserDTOResponse
{
    /// <summary>
    /// Gets or sets a Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets a Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets a phone number
    /// </summary>
    public string? PhoneNumber { get; set; }

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
