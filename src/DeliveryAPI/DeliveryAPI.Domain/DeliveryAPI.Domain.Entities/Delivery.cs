using DeliveryAPI.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryAPI.Domain.Entities;

/// <summary>
/// Delivery
/// </summary>
public class Delivery
{
    /// <summary>
    /// Id
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    /// <summary>
    /// Order Id
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Address Id
    /// </summary>
    public Guid AddressId { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    public Address? Address { get; set; }

    /// <summary>
    /// Courier Id
    /// </summary>
    public Guid? CourierId { get; set; }

    /// <summary>
    /// Courier
    /// </summary>
    public Courier? Courier { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public DeliveryStatus Status { get; set; } = DeliveryStatus.WaitingForTheCourier;
}