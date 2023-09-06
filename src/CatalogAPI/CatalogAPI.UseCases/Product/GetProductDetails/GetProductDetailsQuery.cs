using CatalogAPI.Contracts.DTO;
using MediatR;

namespace CatalogAPI.UseCases.GetProductDetails;

public class GetProductDetailsQuery : IRequest<ProductDTOResponse>
{
    public Guid? Id { get; private set; } = Guid.Empty;

    public string? Name { get; private set; }

    public GetProductDetailsQuery(Guid id)
    {
        Id = id;
    }

    public GetProductDetailsQuery(string name)
    {
        Name = name;
    }
}
