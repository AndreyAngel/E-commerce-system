using CatalogAPI.Contracts.DTO;
using MediatR;

namespace CatalogAPI.UseCases.UpdateProduct;

public class UpdateProductCommand : IRequest
{
    public Guid Id { get; }

    public ProductDTORequest Model { get; }

    public UpdateProductCommand(Guid id, ProductDTORequest model)
    {
        Id = id;
        Model = model;
    }
}
