namespace IdentityAPI.Exceptions;

/// <summary>
/// The exception that is throw when a incorrect password error is detected
/// </summary>
public class IncorrectPasswordException : ArgumentException
{
    public IncorrectPasswordException() : base() { }

    public IncorrectPasswordException(string paramName) : base(paramName) { }

    public IncorrectPasswordException(string message, string paramName) :
        base(message, paramName)
    { }
}
