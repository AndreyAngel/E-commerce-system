using MassTransit;
using Infrastructure.DTO;
using MediatR;
using CatalogAPI.UseCases.GetActualityProducts;

namespace CatalogAPI.Consumers;

/// <summary>
/// Consumer of the check products message
/// </summary>
public class CheckProductsConsumer : IConsumer<ProductListDTORabbitMQ<Guid>>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Creates an instance of the <see cref="CheckProductsConsumer"/>.
    /// </summary>
    /// providing the APIs for managing product in a persistence store </param>
    public CheckProductsConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task Consume(ConsumeContext<ProductListDTORabbitMQ<Guid>> context)
    {
        var content = context.Message;
        var res = _mediator.Send(new GetActualityProductsQuery(content));
        await context.RespondAsync(res);
    }
}
