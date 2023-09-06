using Infrastructure.DTO;
using MediatR;

namespace CatalogAPI.UseCases.GetActualityProducts;

public class GetActualityProductsQuery : IRequest<ProductListDTORabbitMQ<ProductDTORabbitMQ>>
{
    public ProductListDTORabbitMQ<Guid> Model { get; }

    public GetActualityProductsQuery(ProductListDTORabbitMQ<Guid> model)
    {
        Model = model;
    }
}
