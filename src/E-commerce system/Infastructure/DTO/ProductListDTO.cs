namespace Infrastructure.DTO;

public class ProductListDTO<Type>
{
    public List<Type> Products { get; set; } = new List<Type>();
}
