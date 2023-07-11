using MassTransit;
using Infrastructure.DTO;
using Infrastructure.Exceptions;
using DeliveryAPI.UseCases.Interfaces;
using DeliveryAPI.UseCases.Interfaces.Exceptions;

namespace DeliveryAPI.Service.Consumers;

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
