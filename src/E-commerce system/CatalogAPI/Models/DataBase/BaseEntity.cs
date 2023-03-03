namespace CatalogAPI.Models.DataBase;

public class BaseEntity : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    //public DateTime CreationDate { get; set; }
}
