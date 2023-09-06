using CatalogAPI.Contracts.DTO;
using MediatR;

namespace CatalogAPI.UseCases.CreateBrand;

public class CreateBrandCommand : IRequest<BrandDTOResponse>
{
    public CreateBrandCommand(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Brand name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; set; }
}
