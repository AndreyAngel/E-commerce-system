namespace CatalogAPI.Models.DataBase;

public class Brand : BaseEntity
{
    public virtual List<Product> Products { get; set; } = new List<Product>();
}
