namespace OrderAPI.DTO;

public class ProductListDTO<Type>
{
    public List<Type> Products { get; set; } = new List<Type>();
}
