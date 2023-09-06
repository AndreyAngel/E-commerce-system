using CatalogAPI.Contracts.DTO;
using MediatR;

namespace CatalogAPI.UseCases.GetBrandDetails;

public class GetBrandDetailsQuery : IRequest<BrandDTOResponse>
{
    /// <summary>
    /// Brand Id
    /// </summary>
    public Guid? Id { get; } = Guid.Empty;

    /// <summary>
    /// Brand name
    /// </summary>
    public string? Name { get; }

    public GetBrandDetailsQuery(Guid id)
    {
        Id = id;
    }

    public GetBrandDetailsQuery(string name)
    {
        Name = name;
    }
}
