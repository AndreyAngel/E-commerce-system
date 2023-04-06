namespace OrderAPI.Exceptions;

public class EmptyOrderException: ArgumentException
{
    public EmptyOrderException() : base() { }
    public EmptyOrderException(string message): base(message) { }
    public EmptyOrderException(string message, string paramName) :
        base(message, paramName) { }
}
