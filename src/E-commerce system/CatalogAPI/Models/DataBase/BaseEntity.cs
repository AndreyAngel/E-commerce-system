namespace OrderAPI.Models.DataBase;

public class BaseEntity : IEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;
}
