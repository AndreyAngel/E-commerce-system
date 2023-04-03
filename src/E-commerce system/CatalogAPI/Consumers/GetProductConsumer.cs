using AutoMapper;
using OrderAPI.Services.Interfaces;
using OrderAPI.DTO;
using OrderAPI.Exceptions;
using MassTransit;

namespace OrderAPI.Consumers;

public class GetProductConsumer: IConsumer<ProductDTO>
{
    private readonly IProductService _service;
    private readonly IMapper _mapper;

    public GetProductConsumer(IProductService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<ProductDTO> context)
    {
        var content = context.Message;

        try
        {
            var product = _service.GetById(content.Id);
            var result = _mapper.Map<ProductDTO>(product);
            await context.RespondAsync(result);
        }
        catch(NotFoundException ex)
        {
            await context.RespondAsync(new ProductDTO() { Id = content.Id, ErrorMessage = ex.Message});
        }
        catch (ArgumentOutOfRangeException ex)
        {
            await context.RespondAsync(new ProductDTO() { Id = content.Id, ErrorMessage = ex.Message });
        }
        catch(Exception ex)
        {
            await context.RespondAsync(new ProductDTO() { Id = content.Id, ErrorMessage = ex.Message });
        }
    }
}
