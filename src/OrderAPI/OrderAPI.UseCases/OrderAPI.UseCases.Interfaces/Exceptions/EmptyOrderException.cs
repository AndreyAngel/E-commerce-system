namespace OrderAPI.UseCases.Interfaces.Exceptions;

/// <summary>
/// The exception that is throw when a empty order error is detected
/// </summary>
public class EmptyOrderException: ArgumentException
{
    /// <summary>
    /// Creates an instance of the <see cref="EmptyOrderException"/>.
    /// </summary>
    public EmptyOrderException() : base() { }

    /// <summary>
    /// Creates an instance of the <see cref="EmptyOrderException"/>.
    /// </summary>
    /// <param name="message"> Message </param>
    public EmptyOrderException(string message): base(message) { }

    /// <summary>
    /// Creates an instance of the <see cref="EmptyOrderException"/>.
    /// </summary>
    /// <param name="message"> Message </param>
    /// <param name="paramName"> Param name </param>
    public EmptyOrderException(string message, string paramName) : base(message, paramName) { }
}
