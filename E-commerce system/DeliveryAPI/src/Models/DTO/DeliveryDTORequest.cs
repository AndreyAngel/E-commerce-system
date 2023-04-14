namespace DeliveryAPI.Models.DTO;

public class DeliveryDTORequest
{
    public Guid OrderId { get; set; }

    public AddressDTORequest? Address { get; set; }
}
