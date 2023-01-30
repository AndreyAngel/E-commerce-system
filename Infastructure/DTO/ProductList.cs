using Infrastructure.Models;

namespace Infrastructure.DTO;

public class ProductList
{
    public List<Product> Products { get; set; } = new List<Product>();
}
