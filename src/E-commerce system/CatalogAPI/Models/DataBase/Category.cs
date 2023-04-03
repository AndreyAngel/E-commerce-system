namespace OrderAPI.Models.DataBase;

public class Category : BaseEntity
{
    public virtual List<Product> Products { get; set; } = new List<Product>();
}
