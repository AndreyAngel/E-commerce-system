namespace CatalogAPI.Models.DTO;

public class ProductDTOResponce
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public double Price { get; set; }

    public bool IsSale { get; set; }

    public virtual CategoryDTOResponce? Category { get; set; }

    public virtual BrandDTOResponce? Brand { get; set; }
}
