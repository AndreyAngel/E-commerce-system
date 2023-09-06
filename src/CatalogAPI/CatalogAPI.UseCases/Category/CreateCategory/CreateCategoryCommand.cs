using CatalogAPI.Contracts.DTO;
using MediatR;

namespace CatalogAPI.UseCases.CreateCategory;

public class CreateCategoryCommand : IRequest<CategoryDTOResponse>
{
    public CreateCategoryCommand(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Category name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; set; }
}
