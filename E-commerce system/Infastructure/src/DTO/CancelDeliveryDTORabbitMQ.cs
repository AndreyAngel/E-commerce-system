namespace Infrastructure.DTO;

/// <summary>
/// Data transfer object used by RabbitMQ for sending of message about canceled of the order
/// </summary>
public class CancelDeliveryDTORabbitMQ
{
    /// <summary>
    /// Order Id
    /// </summary>
    public Guid OrderId { get; set; }
}
