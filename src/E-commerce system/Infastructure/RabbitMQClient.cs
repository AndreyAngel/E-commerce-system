using MassTransit;

namespace OrderAPI;

public class RabbitMQClient
{
    public static async Task<TResult> Request<TRequest, TResult>(IBusControl bus, TRequest request, Uri uri)
        where TResult : class
        where TRequest: class
    {
        var client = bus.CreateRequestClient<TRequest>(uri);
        var res = await client.GetResponse<TResult>(request);
        return res.Message;
    }

    public static async Task Request<TRequest>(IBusControl bus, TRequest request, Uri uri)
        where TRequest : class
    {
        var sendEnpoint = await bus.GetSendEndpoint(uri);
        await sendEnpoint.Send(request);
    }
}
