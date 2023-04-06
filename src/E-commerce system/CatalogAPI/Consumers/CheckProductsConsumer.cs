using MassTransit;
using Infrastructure.DTO;
using CatalogAPI.Services.Interfaces;

namespace CatalogAPI.Consumers;

public class CheckProductsConsumer : IConsumer<ProductListDTORabbitMQ<Guid>>
{
    private readonly IProductService _service;

    public CheckProductsConsumer(IProductService service)
    {
        _service = service;
    }
    public async Task Consume(ConsumeContext<ProductListDTORabbitMQ<Guid>> context)
    {
        var content = context.Message;
        var res = _service.CheckProducts(content);
        await context.RespondAsync<ProductListDTORabbitMQ<ProductDTORabbitMQ>>(res);
    }
}
