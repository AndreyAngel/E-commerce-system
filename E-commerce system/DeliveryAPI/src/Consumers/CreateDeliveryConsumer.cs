using MassTransit;
using Infrastructure.DTO;
using DeliveryAPI.Services;
using AutoMapper;
using DeliveryAPI.DataBase.Entities;

namespace DeliveryAPI.Consumers;

/// <summary>
/// Consumer of the message about creating of the order with delivery
/// </summary>
public class CreateDeliveryConsumer : IConsumer<DeliveryDTORabbitMQ>
{
    /// <summary>
    /// Object of class <see cref="ICourierService"/> providing the APIs for managing delivery in a persistence store.
    /// </summary>
    private readonly IDeliveryService _service;

    /// <summary>
    /// Object of class <see cref="IMapper"/> for models mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="CreateDeliveryConsumer"/>.
    /// </summary>
    /// <param name="service"> Object of class <see cref="IDeliveryService"/>
    /// <param name="mapper"> Object of class <see cref="IMapper"/> for models mapping
    /// providing the APIs for managing product in a persistence store </param>
    public CreateDeliveryConsumer(IDeliveryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task Consume(ConsumeContext<DeliveryDTORabbitMQ> context)
    {
        var content = context.Message;
        var delivery = _mapper.Map<Delivery>(content);
        await _service.Create(delivery);
    }
}
