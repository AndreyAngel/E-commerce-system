using MassTransit;
using Infrastructure.DTO;
using CatalogAPI.Services;

namespace CatalogAPI.Consumers;

public class ProductsConsumer : IConsumer<ProductList>
{
    private readonly ProductService _service;

    public ProductsConsumer(ProductService service)
    {
        _service = service;
    }
    public async Task Consume(ConsumeContext<ProductList> context)
    {
        var content = context.Message;
        var res = await _service.CheckProducts(content);
        await context.RespondAsync(res);
    }
}
