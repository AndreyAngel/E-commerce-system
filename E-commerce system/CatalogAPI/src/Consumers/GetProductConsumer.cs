using AutoMapper;
using CatalogAPI.Services.Interfaces;
using Infrastructure.DTO;
using Infrastructure.Exceptions;
using MassTransit;

namespace CatalogAPI.Consumers;

public class GetProductConsumer: IConsumer<ProductDTORabbitMQ>
{
    private readonly IProductService _service;
    private readonly IMapper _mapper;

    public GetProductConsumer(IProductService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
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
