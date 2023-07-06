namespace Infrastructure.DTO;

/// <summary>
/// Product data transfer object used by RabbitMQ
/// </summary>
public class ProductDTORabbitMQ
{
    /// <summary>
    /// Product Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Product name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    public double? Price { get; set; }

    /// <summary>
    /// Error message
    /// </summary>
    public string? ErrorMessage { get; set; }
}
