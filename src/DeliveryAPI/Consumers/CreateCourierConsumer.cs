using MassTransit;
using Infrastructure.DTO;
using DeliveryAPI.Services;
using AutoMapper;
using DeliveryAPI.DataBase.Entities;

namespace DeliveryAPI.Consumers;

/// <summary>
/// Consumer of the message about creating of the user with courier role
/// </summary>
public class CreateCourierConsumer : IConsumer<CourierDTORabbitMQ>
{
    /// <summary>
    /// Object of class <see cref="ICourierService"/> providing the APIs for managing courier in a persistence store.
    /// </summary>
    private readonly ICourierService _service;

    /// <summary>
    /// Object of class <see cref="IMapper"/> for models mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="CreateCourierConsumer"/>.
    /// </summary>
    /// <param name="service"> Object of class <see cref="ICourierService"/>
    /// <param name="mapper"> Object of class <see cref="IMapper"/> for models mapping
    /// providing the APIs for managing product in a persistence store </param>
    public CreateCourierConsumer(ICourierService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task Consume(ConsumeContext<CourierDTORabbitMQ> context)
    {
        var content = context.Message;
        var courier = _mapper.Map<Courier>(content);
        await _service.Create(courier);
    }
}
