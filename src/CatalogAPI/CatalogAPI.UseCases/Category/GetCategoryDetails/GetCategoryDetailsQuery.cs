using CatalogAPI.Contracts.DTO;
using MediatR;

namespace CatalogAPI.UseCases.GetCategoryDetails;

public class GetCategoryDetailsQuery : IRequest<CategoryDTOResponse>
{
    /// <summary>
    /// Category Id
    /// </summary>
    public Guid? Id { get; } = Guid.Empty;

    /// <summary>
    /// Category name
    /// </summary>
    public string? Name { get; }

    public GetCategoryDetailsQuery(Guid id)
    {
        Id = id;
    }

    public GetCategoryDetailsQuery(string name)
    {
        Name = name;
    }
}
