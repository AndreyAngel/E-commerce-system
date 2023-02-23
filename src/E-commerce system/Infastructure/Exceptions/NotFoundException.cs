namespace Infrastructure.Exceptions;

public class NotFoundException: ArgumentException
{
    public NotFoundException() : base() { }
    public NotFoundException(string paramName) : base(paramName) { }
    public NotFoundException(string paramName, string message) :
        base(paramName, message) { }
}
