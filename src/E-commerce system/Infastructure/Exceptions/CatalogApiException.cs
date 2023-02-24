namespace Infrastructure.Exceptions;

public class CatalogApiException: ArgumentException
{
    public CatalogApiException() : base() { }
    public CatalogApiException(string message): base(message) { }
    public CatalogApiException(string paramName, string message) :
        base(paramName, message) { }
}
