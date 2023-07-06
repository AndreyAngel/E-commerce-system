namespace OrderAPI.UseCases.Interfaces.Exceptions;

/// <summary>
/// The exception that is throw when a order status error is detected
/// </summary>
public class OrderStatusException: ArgumentException
{
    /// <summary>
    /// Creates an instance of the <see cref="OrderStatusException"/>.
    /// </summary>
    public OrderStatusException() : base() { }

    /// <summary>
    /// Creates an instance of the <see cref="OrderStatusException"/>.
    /// </summary>
    /// <param name="message"> Message </param>
    public OrderStatusException(string message): base(message) { }

    /// <summary>
    /// Creates an instance of the <see cref="OrderStatusException"/>.
    /// </summary>
    /// <param name="message"> Message </param>
    /// <param name="paramName"> Param name </param>
    public OrderStatusException(string message, string paramName) :
        base(message, paramName) { }
}
