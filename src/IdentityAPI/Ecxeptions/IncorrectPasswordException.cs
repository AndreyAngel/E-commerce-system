namespace IdentityAPI.Exceptions;

/// <summary>
/// The exception that is throw when a incorrect password error is detected
/// </summary>
public class IncorrectPasswordException : ArgumentException
{
    /// <summary>
    /// Creates an instance of the <see cref="IncorrectPasswordException"/>.
    /// </summary>
    public IncorrectPasswordException() : base() { }

    /// <summary>
    /// Creates an instance of the <see cref="IncorrectPasswordException"/>.
    /// </summary>
    /// <param name="paramName"> Param name </param>
    public IncorrectPasswordException(string paramName) : base(paramName) { }

    /// <summary>
    /// Creates an instance of the <see cref="IncorrectPasswordException"/>.
    /// </summary>
    /// <param name="message"> Message </param>
    /// <param name="paramName"> Param name </param>
    public IncorrectPasswordException(string message, string paramName) : base(message, paramName)
    { }
}
