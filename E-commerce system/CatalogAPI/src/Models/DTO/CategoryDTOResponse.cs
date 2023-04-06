namespace CatalogAPI.Models.DTO;

public class CategoryDTOResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}
