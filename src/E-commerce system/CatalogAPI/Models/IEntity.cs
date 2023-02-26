namespace CatalogAPI.Models;

public interface IEntity
{
    int Id { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    //DateTime CreationDate { get; set; }
}
