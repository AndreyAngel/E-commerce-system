using MassTransit;

namespace Infrastructure;

/// <summary>
/// The RabbitMQ client through which services communicate
/// </summary>
public class RabbitMQClient
{
    /// <summary>
    /// Handles requests waiting for a response
    /// </summary>
    /// <typeparam name="TRequest"> Type of request </typeparam>
    /// <typeparam name="TResult"> Type of response </typeparam>
    /// <param name="bus"> <see cref="IBusControl"/> </param>
    /// <param name="request"> Request </param>
    /// <param name="uri"> Consumer URI </param>
    /// <returns> Task object containing of response </returns>
    public static async Task<TResult> Request<TRequest, TResult>(IBusControl bus, TRequest request, Uri uri)
        where TResult : class
        where TRequest: class
    {
        var client = bus.CreateRequestClient<TRequest>(uri);
        var res = await client.GetResponse<TResult>(request);
        return res.Message;
    }

    /// <summary>
    /// Handles requests not waiting for a response
    /// </summary>
    /// <typeparam name="TRequest"> Type of request </typeparam>
    /// <param name="bus"> <see cref="IBusControl"/> </param>
    /// <param name="request"> Request </param>
    /// <param name="uri"> Cunsumer URI </param>
    /// <returns> Task object </returns>
    public static async Task Request<TRequest>(IBusControl bus, TRequest request, Uri uri)
        where TRequest : class
    {
        var sendEnpoint = await bus.GetSendEndpoint(uri);
        await sendEnpoint.Send(request);
    }
}
