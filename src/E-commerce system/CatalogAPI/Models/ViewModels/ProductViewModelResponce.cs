namespace CatalogAPI.Models.ViewModels;

public class ProductViewModelResponce
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public double Price { get; set; }

    public virtual CategoryViewModelResponce? Category { get; set; }

    public virtual BrandViewModelResponce? Brand { get; set; }
}
