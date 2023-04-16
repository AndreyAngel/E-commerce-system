using DeliveryAPI.DataBase;

namespace DeliveryAPI.Models.DTO;

public class DeliveryFilterDTORequest
{
    public DeliveryStatus? Status { get; set; }

    public Guid? CourierId { get; set; }

    public AddressDTO? Address { get; set; }
}
