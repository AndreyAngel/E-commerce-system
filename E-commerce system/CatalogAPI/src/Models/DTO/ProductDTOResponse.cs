namespace CatalogAPI.Models.DTO;

public class ProductDTOResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public double Price { get; set; }

    public bool IsSale { get; set; }

    public virtual CategoryDTOResponse? Category { get; set; }

    public virtual BrandDTOResponse? Brand { get; set; }
}
