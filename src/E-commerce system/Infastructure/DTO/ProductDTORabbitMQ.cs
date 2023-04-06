namespace Infrastructure.DTO;

public class ProductDTORabbitMQ
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public double? Price { get; set; }
    public string? ErrorMessage { get; set; }
}
