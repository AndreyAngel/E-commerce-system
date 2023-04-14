namespace Infrastructure.DTO;

public class DeliveryDTORabbitMQ
{
    public Guid OrderId { get; set; }

    public AddressDTORabbitMQ? Address { get; set; }
}
