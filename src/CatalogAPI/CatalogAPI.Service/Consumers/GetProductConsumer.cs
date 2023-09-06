using AutoMapper;
using CatalogAPI.Contracts.DTO;
using CatalogAPI.UseCases.GetProductDetails;
using Infrastructure.DTO;
using Infrastructure.Exceptions;
using MassTransit;
using MediatR;

namespace CatalogAPI.Consumers;

/// <summary>
/// Consumer of the get products message
/// </summary>
public class GetProductConsumer: IConsumer<ProductDTORabbitMQ>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Object of class <see cref="IMapper"/> for models mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="GetProductConsumer"/>.
    /// </summary>
    public GetProductConsumer(IMediator mediator)
    {
        _mediator = mediator;
        _mapper = new Mapper(CreateMapperConfiguration());
    }

    /// <inheritdoc/>
    public async Task Consume(ConsumeContext<ProductDTORabbitMQ> context)
    {
        var content = context.Message;

        try
        {
            var product = await _mediator.Send(new GetProductDetailsQuery(content.Id));
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

    /// <summary>
    /// Create mapper configuration
    /// </summary>
    /// <returns><see cref="MapperConfiguration"/></returns>
    private static MapperConfiguration CreateMapperConfiguration()
    {
        return new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductDTOResponse, ProductDTORabbitMQ>();
        });
    }
}
