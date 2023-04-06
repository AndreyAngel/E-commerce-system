namespace Infrastructure.DTO;

public class ProductListDTORabbitMQ<Type>
{
    public List<Type> Products { get; set; } = new List<Type>();
}
