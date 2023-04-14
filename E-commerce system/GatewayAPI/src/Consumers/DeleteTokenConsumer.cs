using GatewayAPI.Authorization;
using Infrastructure.DTO;
using MassTransit;

namespace GatewayAPI.Consumers;

/// <summary>
/// Consume of the message about deleting token
/// </summary>
public class DeleteTokenConsumer : IConsumer<TokenDTORAbbitMQ>
{
    /// <summary>
    /// Interface for class providing the APIs for managing token in a persistence store.
    /// </summary>
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Creates an instance of the <see cref="AddTokenConsumer"/>
    /// </summary>
    /// <param name="tokenService"> Interface for class providing the APIs for managing token in a persistence store </param>
    public DeleteTokenConsumer(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    /// <inheritdoc/>
    public async Task Consume(ConsumeContext<TokenDTORAbbitMQ> context)
    {
        var content = context.Message;
        await _tokenService.DeleteToken(content.Value);
    }
}
