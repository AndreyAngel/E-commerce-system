using CatalogAPI.Contracts.DTO;
using MediatR;

namespace CatalogAPI.UseCases.CreateProduct;

public class CreateProductCommand : IRequest<ProductDTOResponse>
{
    public ProductDTORequest Model { get; }

    public CreateProductCommand(ProductDTORequest model)
    {
        Model = model;
    }
}
