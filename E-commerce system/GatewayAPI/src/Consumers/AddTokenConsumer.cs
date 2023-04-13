using GatewayAPI.Authorization;
using Infrastructure.DTO;
using MassTransit;

namespace GatewayAPI.Consumers;

public class AddTokenConsumer : IConsumer<TokenDTO>
{
    private readonly ITokenService _tokenService;

    public AddTokenConsumer(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task Consume(ConsumeContext<TokenDTO> context)
    {
        var content = context.Message;
        await _tokenService.AddToken(content.Value);
    }
}
