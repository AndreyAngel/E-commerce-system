namespace Infrastructure.DTO;

/// <summary>
/// Delivery data transfer object used by RabbitMQ for creating of the delivery
/// </summary>
public class DeliveryDTORabbitMQ
{
    /// <summary>
    /// Order Id
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Address data transfer object used by RabbitMQ
    /// </summary>
    public AddressDTORabbitMQ? Address { get; set; }
}
