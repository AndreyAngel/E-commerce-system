using Infrastructure.DTO;
using MassTransit;
using OrderAPI.Services.Interfaces;

namespace OrderAPI.Consumers;

public class CreateCartConsumer : IConsumer<CartDTORabbitMQ>
{
    public readonly ICartService _cartService;

    public CreateCartConsumer(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task Consume(ConsumeContext<CartDTORabbitMQ> context)
    {
        var content = context.Message;
        await _cartService.Create(content.Id);
    }
}
