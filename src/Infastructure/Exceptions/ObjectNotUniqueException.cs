namespace Infrastructure.Exceptions;

/// <summary>
/// The exception that is throw when a of the object not unique is detected
/// </summary>
public class ObjectNotUniqueException: ArgumentException
{
    /// <summary>
    /// Creates an instance of the <see cref="ObjectNotUniqueException"/>.
    /// </summary>
    public ObjectNotUniqueException() : base() { }

    /// <summary>
    /// Creates an instance of the <see cref="ObjectNotUniqueException"/>.
    /// </summary>
    /// <param name="paramName"> Paran name </param>
    public ObjectNotUniqueException(string paramName) : base(paramName) { }

    /// <summary>
    /// Creates an instance of the <see cref="ObjectNotUniqueException"/>.
    /// </summary>
    /// <param name="message"> Message </param>
    /// <param name="paramName"> Param name </param>
    public ObjectNotUniqueException(string message, string paramName) : base(message, paramName) { }
}
