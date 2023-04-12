namespace Infrastructure.Exceptions;

/// <summary>
/// The exception that is throw when a error from CatalogAPI service is detected
/// </summary>
public class CatalogApiException: ArgumentException
{
    /// <summary>
    /// Creates an instance of the <see cref="CatalogApiException"/>.
    /// </summary>
    public CatalogApiException() : base() { }

    /// <summary>
    /// Creates an instance of the <see cref="CatalogApiException"/>.
    /// </summary>
    /// <param name="message"> Message </param>
    public CatalogApiException(string message): base(message) { }

    /// <summary>
    /// Creates an instance of the <see cref="CatalogApiException"/>.
    /// </summary>
    /// <param name="message"> Message </param>
    /// <param name="paramName"> Param name </param>
    public CatalogApiException(string message, string paramName) : base(message, paramName) { }
}
