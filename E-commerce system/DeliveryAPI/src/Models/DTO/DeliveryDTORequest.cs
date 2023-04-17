using System.ComponentModel.DataAnnotations;

namespace DeliveryAPI.Models.DTO;

/// <summary>
/// The delivery data transfer object as a request
/// </summary>
public class DeliveryDTORequest
{
    /// <summary>
    /// Order Id
    /// </summary>
    [Required]
    public Guid OrderId { get; set; }

    /// <summary>
    /// Address data transfer object
    /// </summary>
    [Required]
    public AddressDTO? Address { get; set; }
}
