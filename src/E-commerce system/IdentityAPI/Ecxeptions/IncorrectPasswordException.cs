namespace IdentityAPI.Exceptions;

public class IncorrectPasswordException : ArgumentException
{
    public IncorrectPasswordException() : base() { }
    public IncorrectPasswordException(string paramName) : base(paramName) { }
    public IncorrectPasswordException(string message, string paramName) :
        base(message, paramName)
    { }
}
