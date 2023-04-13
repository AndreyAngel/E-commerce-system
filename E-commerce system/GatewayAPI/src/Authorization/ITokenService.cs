namespace GatewayAPI.Authorization;

public interface ITokenService : IDisposable
{
    bool IsActive(string token);

    Task AddToken(string tokenValue);

    Task DeleteToken(string tokenValue);
}
