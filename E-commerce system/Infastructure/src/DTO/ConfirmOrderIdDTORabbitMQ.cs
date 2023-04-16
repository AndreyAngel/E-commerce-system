namespace Infrastructure.DTO;

public class ConfirmOrderIdDTORabbitMQ
{
    public Guid? OrderId { get; set; } = null;

    public string? Error { get; set; } = null;
}
