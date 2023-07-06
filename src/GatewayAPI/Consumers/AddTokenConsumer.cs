using GatewayAPI.Authorization;
using Infrastructure.DTO;
using MassTransit;

namespace GatewayAPI.Consumers;

/// <summary>
/// Consume of the message about adding new token
/// </summary>
public class AddTokenConsumer : IConsumer<TokenDTORAbbitMQ>
{
    /// <summary>
    /// Interface for class providing the APIs for managing token in a persistence store.
    /// </summary>
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Creates an instance of the <see cref="AddTokenConsumer"/>
    /// </summary>
    /// <param name="tokenService"> Interface for class providing the APIs for managing token in a persistence store </param>
    public AddTokenConsumer(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    /// <inheritdoc/>
    public async Task Consume(ConsumeContext<TokenDTORAbbitMQ> context)
    {
        var content = context.Message;
        await _tokenService.AddToken(content.Value);
    }
}
