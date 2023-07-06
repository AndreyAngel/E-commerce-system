using Infrastructure.DTO;
using MassTransit;
using OrderAPI.UseCases.Interfaces;

namespace OrderAPI.Service.Consumers;

/// <summary>
/// Consumer of the message for create cart
/// </summary>
public class OrderIsReceivedConsumer : IConsumer<OrderIsReceivedDTORabbitMQ>
{
    /// <summary>
    /// Interface for class providing the APIs for managing order in a persistence store.
    /// </summary>
    public readonly IOrderService _orderService;

    /// <summary>
    /// Creates an instance of the <see cref="CreateCartConsumer"/>.
    /// </summary>
    /// <param name="orderService"> Interface for class providing the APIs for managing order in a persistence store. </param>
    public OrderIsReceivedConsumer(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <inheritdoc/>
    public async Task Consume(ConsumeContext<OrderIsReceivedDTORabbitMQ> context)
    {
        var content = context.Message;
        await _orderService.IsReceived(content.OrderId);
    }
}
