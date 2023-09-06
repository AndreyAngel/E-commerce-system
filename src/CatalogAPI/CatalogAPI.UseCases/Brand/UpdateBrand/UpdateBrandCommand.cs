using MediatR;

namespace CatalogAPI.UseCases.UpdateBrand;

public class UpdateBrandCommand : IRequest
{
    public UpdateBrandCommand(Guid id, string? name, string? description)
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
