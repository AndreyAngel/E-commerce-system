using DeliveryAPI.DataBase;

namespace DeliveryAPI.Models.DTO;

/// <summary>
/// The delivery data transfer object as a response
/// </summary>
public class DeliveryDTOResponse
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Order Id
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Address data transfer object
    /// </summary>
    public AddressDTO Address { get; set; }

    /// <summary>
    /// The courier data transfer object as a response
    /// </summary>
    public CourierDTOResponse? Courier { get; set; }

    /// <summary>
    /// Delivery status
    /// </summary>
    public DeliveryStatus Status { get; set; }
}
