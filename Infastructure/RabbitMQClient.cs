using MassTransit;

namespace Infrastructure;

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
}
