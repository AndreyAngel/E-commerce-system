using DeliveryAPI.DataBase;
using DeliveryAPI.DataBase.Entities;

namespace DeliveryAPI.Models.DTO;

public class DeliveryDTOResponse
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Address Address { get; set; }

    public Courier? Courier { get; set; }

    public DeliveryStatus Status { get; set; }
}
