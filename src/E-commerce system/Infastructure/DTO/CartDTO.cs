namespace OrderAPI.DTO;

public class CartDTO
{
    public Guid Id { get; set; }

    public CartDTO(Guid id)
    {
        Id = id;
    }
}
