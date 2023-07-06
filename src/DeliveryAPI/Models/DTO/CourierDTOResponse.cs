namespace DeliveryAPI.Models.DTO;

/// <summary>
/// The courier data transfer object as a response
/// </summary>
public class CourierDTOResponse
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string PhoneNumber { get; set; }
}
