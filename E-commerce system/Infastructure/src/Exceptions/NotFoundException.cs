namespace Infrastructure.Exceptions;

/// <summary>
/// The exception that is throw when a not found object error is detected
/// </summary>
public class NotFoundException: ArgumentException
{
    /// <summary>
    /// Creates an instance of the <see cref="NotFoundException"/>.
    /// </summary>
    public NotFoundException() : base() { }

    /// <summary>
    /// Creates an instance of the <see cref="NotFoundException"/>.
    /// </summary>
    /// <param name="paramName"> Param name </param>
    public NotFoundException(string paramName) : base(paramName) { }

    /// <summary>
    /// Creates an instance of the <see cref="NotFoundException"/>.
    /// </summary>
    /// <param name="message"> Param name </param>
    /// <param name="paramName"> Message </param>
    public NotFoundException(string message, string paramName) : base(message, paramName) { }
}
