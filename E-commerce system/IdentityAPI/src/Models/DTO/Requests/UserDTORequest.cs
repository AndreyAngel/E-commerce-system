namespace IdentityAPI.Models.DTO.Requests;

/// <summary>
/// Data transfer object for changing user data
/// </summary>
public class UserDTORequest
{
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
