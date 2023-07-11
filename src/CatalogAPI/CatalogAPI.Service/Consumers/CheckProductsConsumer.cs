using MassTransit;
using Infrastructure.DTO;
using CatalogAPI.UseCases.Interfaces;

namespace CatalogAPI.Consumers;

/// <summary>
/// Consumer of the check products message
/// </summary>
public class CheckProductsConsumer : IConsumer<ProductListDTORabbitMQ<Guid>>
{
    /// <summary>
    /// Object of class <see cref="IProductService"/> providing the APIs for managing product in a persistence store.
    /// </summary>
    private readonly IProductService _service;

    /// <summary>
    /// Creates an instance of the <see cref="CheckProductsConsumer"/>.
    /// </summary>
    /// <param name="service"> Object of class <see cref="IProductService"/>
    /// providing the APIs for managing product in a persistence store </param>
    public CheckProductsConsumer(IProductService service)
    {
        _service = service;
    }

    /// <inheritdoc/>
    public async Task Consume(ConsumeContext<ProductListDTORabbitMQ<Guid>> context)
    {
        var content = context.Message;
        var res = _service.GetActualityProducts(content);
        await context.RespondAsync(res);
    }
}
