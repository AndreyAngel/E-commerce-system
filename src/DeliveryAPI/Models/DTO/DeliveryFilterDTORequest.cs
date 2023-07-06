using DeliveryAPI.DataBase;

namespace DeliveryAPI.Models.DTO;

/// <summary>
/// The delivery filter data transfer object as a request
/// </summary>
public class DeliveryFilterDTORequest
{
    /// <summary>
    /// Delivery ststus
    /// </summary>
    public DeliveryStatus? Status { get; set; }

    /// <summary>
    /// Courier Id
    /// </summary>
    public Guid? CourierId { get; set; }

    /// <summary>
    /// Address data transfer object
    /// </summary>
    public AddressDTO? Address { get; set; }
}
