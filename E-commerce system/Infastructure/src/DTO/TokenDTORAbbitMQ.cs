namespace Infrastructure.DTO;

/// <summary>
/// Token data transfer object used by RabbitMQ
/// </summary>
public class TokenDTORAbbitMQ
{
    /// <summary>
    /// Gets or set a token value
    /// </summary>
    public string Value { get; init; }
}
