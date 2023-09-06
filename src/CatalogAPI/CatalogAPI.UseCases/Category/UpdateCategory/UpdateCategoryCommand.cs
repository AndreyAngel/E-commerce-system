using MediatR;

namespace CatalogAPI.UseCases.UpdateCategory;

public class UpdateCategoryCommand : IRequest
{
    public UpdateCategoryCommand(Guid id, string? name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Brand Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Brand name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; set; }
}
