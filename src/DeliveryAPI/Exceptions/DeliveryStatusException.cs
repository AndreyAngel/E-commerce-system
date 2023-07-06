namespace OrderAPI.Exceptions;

/// <summary>
/// The exception that is throw when a delivery status error is detected
/// </summary>
public class DeliveryStatusException: ArgumentException
{
    /// <summary>
    /// Creates an instance of the <see cref="DeliveryStatusException"/>.
    /// </summary>
    public DeliveryStatusException() : base() { }

    /// <summary>
    /// Creates an instance of the <see cref="DeliveryStatusException"/>.
    /// </summary>
    /// <param name="message"> Message </param>
    public DeliveryStatusException(string message): base(message) { }

    /// <summary>
    /// Creates an instance of the <see cref="DeliveryStatusException"/>.
    /// </summary>
    /// <param name="message"> Message </param>
    /// <param name="paramName"> Param name </param>
    public DeliveryStatusException(string message, string paramName) :
        base(message, paramName) { }
}
