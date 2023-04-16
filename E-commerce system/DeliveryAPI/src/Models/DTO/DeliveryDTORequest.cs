using System.ComponentModel.DataAnnotations;

namespace DeliveryAPI.Models.DTO;

public class DeliveryDTORequest
{
    [Required]
    public Guid OrderId { get; set; }

    public AddressDTO Address { get; set; }
}
