namespace Infrastructure.DTO;

/// <summary>
/// Data transfer object used by RabbitMQ for sending of message about received of the order by customer
/// </summary>
public class OrderIsReceivedDTORabbitMQ
{
    /// <summary>
    /// Order Id
    /// </summary>
    public Guid OrderId { get; set; }
}
