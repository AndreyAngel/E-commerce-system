namespace OrderAPI.Models.DataBase;

public interface IEntity
{
    Guid Id { get; set; }

    string Name { get; set; }

    string? Description { get; set; }

    DateTime CreationDate { get; set; }
}
