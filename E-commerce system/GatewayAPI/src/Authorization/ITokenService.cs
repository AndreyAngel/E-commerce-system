namespace GatewayAPI.Authorization;

/// <summary>
/// Interface for class providing the APIs for managing token in a persistence store.
/// </summary>
public interface ITokenService : IDisposable
{
    /// <summary>
    /// Token status
    /// </summary>
    /// <param name="token"> Token value </param>
    /// <returns> True if token is active, flase if token isn't active </returns>
    bool IsActive(string token);

    /// <summary>
    /// Add new token
    /// </summary>
    /// <param name="tokenValue"> Token value </param>
    /// <returns> Task object </returns>
    Task AddToken(string tokenValue);

    /// <summary>
    /// Token property of IsActive = false
    /// </summary>
    /// <param name="tokenValue"> Token value </param>
    /// <returns> Task object </returns>
    Task DeleteToken(string tokenValue);
}
