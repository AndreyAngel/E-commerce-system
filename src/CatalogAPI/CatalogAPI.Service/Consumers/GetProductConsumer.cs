using AutoMapper;
using CatalogAPI.UseCases.Interfaces;
using Infrastructure.DTO;
using Infrastructure.Exceptions;
using MassTransit;

namespace CatalogAPI.Consumers;

/// <summary>
/// Consumer of the get products message
/// </summary>
public class GetProductConsumer: IConsumer<ProductDTORabbitMQ>
{
    /// <summary>
    /// Object of class <see cref="IProductService"/> providing the APIs for managing product in a persistence store.
    /// </summary>
    private readonly IProductService _service;

    /// <summary>
    /// Object of class <see cref="IMapper"/> for models mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="GetProductConsumer"/>.
    /// </summary>
    /// <param name="service"> Object of class <see cref="IProductService"/>
    /// <param name="mapper"> Object of class <see cref="IMapper"/> for models mapping </param>
    public GetProductConsumer(IProductService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task Consume(ConsumeContext<ProductDTORabbitMQ> context)
    {
        var content = context.Message;

        try
        {
            var product = _service.GetById(content.Id);
            var result = _mapper.Map<ProductDTORabbitMQ>(product);
            await context.RespondAsync(result);
        }
        catch(NotFoundException ex)
        {
            await context.RespondAsync(new ProductDTORabbitMQ() { Id = content.Id, ErrorMessage = ex.Message});
        }
        catch (ArgumentOutOfRangeException ex)
        {
            await context.RespondAsync(new ProductDTORabbitMQ() { Id = content.Id, ErrorMessage = ex.Message });
        }
        catch(Exception ex)
        {
            await context.RespondAsync(new ProductDTORabbitMQ() { Id = content.Id, ErrorMessage = ex.Message });
        }
    }
}
