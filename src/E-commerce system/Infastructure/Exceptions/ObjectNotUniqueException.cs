namespace Infrastructure.Exceptions;

public class ObjectNotUniqueException: ArgumentException
{
    public ObjectNotUniqueException() : base() { }
    public ObjectNotUniqueException(string paramName) : base(paramName) { }
    public ObjectNotUniqueException(string paramName, string message)
            : base(message, paramName) { }
}
