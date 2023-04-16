using Infrastructure.DTO;
using Infrastructure.Exceptions;
using MassTransit;
using OrderAPI.Services.Interfaces;

namespace OrderAPI.Consumers;

/// <summary>
/// Consumer of the message for create cart
/// </summary>
public class ConfirmOrderIdConsumer : IConsumer<ConfirmOrderIdDTORabbitMQ>
{
    /// <summary>
    /// Interface for class providing the APIs for managing order in a persistence store.
    /// </summary>
    public readonly IOrderService _orderService;

    /// <summary>
    /// Creates an instance of the <see cref="CreateCartConsumer"/>.
    /// </summary>
    /// <param name="orderService"> Interface for class providing the APIs for managing order in a persistence store. </param>
    public ConfirmOrderIdConsumer(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <inheritdoc/>
    public async Task Consume(ConsumeContext<ConfirmOrderIdDTORabbitMQ> context)
    {
        try
        {
            var content = context.Message;
            _orderService.GetById(content.OrderId.Value);
            await context.RespondAsync(new ConfirmOrderIdDTORabbitMQ() { OrderId = content.OrderId.Value });
        }
        catch (NotFoundException ex)
        {
            await context.RespondAsync(new ConfirmOrderIdDTORabbitMQ() { Error = ex.Message });
        }
    }
}
