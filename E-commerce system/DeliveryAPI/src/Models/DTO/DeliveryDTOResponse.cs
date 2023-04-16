using DeliveryAPI.DataBase;

namespace DeliveryAPI.Models.DTO;

public class DeliveryDTOResponse
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public AddressDTO Address { get; set; }

    public CourierDTOResponse? Courier { get; set; }

    public DeliveryStatus Status { get; set; }
}
