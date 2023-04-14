using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryAPI.DataBase.Entities;

public class Courier
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? Phone { get; set; }

    public List<Delivery> Deliveries { get; set; } = new();
}