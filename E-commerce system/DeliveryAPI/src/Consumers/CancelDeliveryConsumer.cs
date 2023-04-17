using MassTransit;
using Infrastructure.DTO;
using DeliveryAPI.Services;
using Infrastructure.Exceptions;
using OrderAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace DeliveryAPI.Consumers;

/// <summary>
/// Consumer of the message about canceled of the order
/// </summary>
public class CancelDeliveryConsumer : IConsumer<CancelDeliveryDTORabbitMQ>
{
    /// <summary>
    /// Object of class <see cref="ICourierService"/> providing the APIs for managing delivery in a persistence store.
    /// </summary>
    private readonly IDeliveryService _service;

    /// <summary>
    /// Creates an instance of the <see cref="CreateDeliveryConsumer"/>.
    /// </summary>
    /// providing the APIs for managing product in a persistence store </param>
    public CancelDeliveryConsumer(IDeliveryService service)
    {
        _service = service;
    }

    /// <inheritdoc/>
    public async Task Consume(ConsumeContext<CancelDeliveryDTORabbitMQ> context)
    {
        try
        {
            var content = context.Message;
            _service.Cancel(content.OrderId);
        }
        catch (NotFoundException) { }
        catch (DeliveryStatusException) { }
    }
}
