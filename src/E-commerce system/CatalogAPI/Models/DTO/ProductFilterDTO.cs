namespace CatalogAPI.Models.DTO;

public class ProductFilterDTO
{
    public Guid? CategoryId { get; set; }

    public Guid? BrandId { get; set; }
}
