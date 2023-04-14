using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryAPI.DataBase.Entities;

public class Delivery
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid AddressId { get; set; }

    public Address? Address { get; set; }

    public Guid? CourierId { get; set; }

    public Courier? Courier { get; set; }

    public DeliveryStatus Status { get; set; } = DeliveryStatus.WaitingForTheCourier;
}