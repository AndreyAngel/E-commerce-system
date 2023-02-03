using Infrastructure.Models;

namespace Infrastructure.DTO;

public class ProductList<Type>
{
    public List<Type> Products { get; set; } = new List<Type>();
}
