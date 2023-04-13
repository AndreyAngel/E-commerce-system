namespace IdentityAPI.Models.DTO;

/// <summary>
/// The data transfer object of the response containing the user data
/// </summary>
public class UserDTOResponse : IDTOResponse
{
    /// <summary>
    /// User Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Surname
    /// </summary>
    public string? Surname { get; set; }

    /// <summary>
    /// BirthDate
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Address data transfer object
    /// </summary>
    public AddressDTO? Address { get; set; }
}
