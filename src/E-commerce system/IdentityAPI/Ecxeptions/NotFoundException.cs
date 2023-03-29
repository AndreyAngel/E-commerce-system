namespace IdentityAPI.Exceptions;

public class NotFoundException: ArgumentException
{
    public NotFoundException() : base() { }
    public NotFoundException(string paramName) : base(paramName) { }
    public NotFoundException(string message, string paramName) :
        base(message, paramName) { }
}
