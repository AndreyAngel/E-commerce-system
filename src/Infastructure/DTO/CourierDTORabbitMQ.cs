namespace Infrastructure.DTO;

/// <summary>
/// Courier data transfer object used by RabbitMQ
/// </summary>
public class CourierDTORabbitMQ
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string PhoneNumber { get; set; }
}
