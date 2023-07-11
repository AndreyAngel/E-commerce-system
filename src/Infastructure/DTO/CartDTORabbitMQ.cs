namespace Infrastructure.DTO;

/// <summary>
/// Cart data transfer object used by RabbitMQ
/// </summary>
public class CartDTORabbitMQ
{
    /// <summary>
    /// Cart ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Creates an instance of the <see cref="CartDTORabbitMQ"/>.
    /// </summary>
    /// <param name="id"></param>
    public CartDTORabbitMQ(Guid id)
    {
        Id = id;
    }
}
