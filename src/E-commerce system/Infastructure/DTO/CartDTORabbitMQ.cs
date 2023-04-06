namespace Infrastructure.DTO;

public class CartDTORabbitMQ
{
    public Guid Id { get; set; }

    public CartDTORabbitMQ(Guid id)
    {
        Id = id;
    }
}
