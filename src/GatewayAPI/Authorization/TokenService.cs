using GatewayAPI.DataBase;
using Microsoft.EntityFrameworkCore;

namespace GatewayAPI.Authorization;

/// <summary>
/// Class providing the APIs for managing token in a persistence store.
/// </summary>
public class TokenService : ITokenService
{
    /// <summary>
    /// Database context
    /// </summary>
    private readonly Context _context;

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed = false;

    /// <summary>
    /// Creates an instance of the <see cref="TokenService"/>.
    /// </summary>
    /// <param name="context"> Database context </param>
    public TokenService(Context context)
    {
        _context = context;
    }

    ~TokenService() => Dispose(false);

    /// <inheritdoc/>
    public bool IsActive(string token)
    {
        ThrowIfDisposed();

        var res = _context.Tokens.FirstOrDefault(x => x.Value == token);

        if (res == null)
        {
            return false;
        }

        return res.IsActive;
    }

    /// <inheritdoc/>
    public async Task AddToken(string tokenValue)
    {
        var token = new Token()
        {
            Value = tokenValue,
            IsActive = true
        };

        await _context.Tokens.AddAsync(token);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteToken(string tokenValue)
    {
        var token = await _context.Tokens.FirstAsync(x => x.Value == tokenValue);

        if (token != null)
        {
            token.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
}
