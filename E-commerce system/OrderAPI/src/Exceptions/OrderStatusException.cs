namespace OrderAPI.Exceptions;

public class OrderStatusException: ArgumentException
{
    public OrderStatusException() : base() { }

    public OrderStatusException(string message): base(message) { }

    public OrderStatusException(string message, string paramName) :
        base(message, paramName) { }
}
