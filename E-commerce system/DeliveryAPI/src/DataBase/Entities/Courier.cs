using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryAPI.DataBase.Entities;

/// <summary>
/// Information about courier
/// </summary>
public class Courier
{
    /// <summary>
    /// Id
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Deliveries
    /// </summary>
    public List<Delivery> Deliveries { get; set; } = new();
}