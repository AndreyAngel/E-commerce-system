using Infrastructure.DTO;
using MassTransit;
using OrderAPI.UseCases.Interfaces;

namespace OrderAPI.Service.Consumers;

/// <summary>
/// Consumer of the message for create cart
/// </summary>
public class CreateCartConsumer : IConsumer<CartDTORabbitMQ>
{
    /// <summary>
    /// Interface for class providing the APIs for managing cart in a persistence store.
    /// </summary>
    public readonly ICartService _cartService;

    /// <summary>
    /// Creates an instance of the <see cref="CreateCartConsumer"/>.
    /// </summary>
    /// <param name="cartService"> Interface for class providing the APIs for managing cart in a persistence store. </param>
    public CreateCartConsumer(ICartService cartService)
    {
        _cartService = cartService;
    }

    /// <inheritdoc/>
    public async Task Consume(ConsumeContext<CartDTORabbitMQ> context)
    {
        var content = context.Message;
        await _cartService.Create(content.Id);
    }
}
