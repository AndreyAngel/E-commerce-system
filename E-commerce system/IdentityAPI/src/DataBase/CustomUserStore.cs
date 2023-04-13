using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityAPI.Models.Enums;
using IdentityAPI.DataBase.Entities;

namespace IdentityAPI.DataBase;

/// <summary>
/// Represents a new instance of a persistence store for users, using the default implementation
/// </summary>
public class CustomUserStore : UserStore<User>, ICustomUserStore, IDisposable
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
    /// Gets database context
    /// </summary>
    public override Context Context { get => _context; }

    /// <summary>
    /// Creates an instance of the <see cref="CustomUserStore"/>.
    /// </summary>
    /// <param name="context"></param>
    public CustomUserStore(Context context) : base(context)
    {
        AutoSaveChanges = true;
        _context = context;
    }

    ~CustomUserStore() => Dispose(false);

    /// <inheritdoc/>
    public async Task AddRangeTokenAsync(IEnumerable<Token> tokens)
    {
        ThrowIfDisposed();
        await _context.Tokens.AddRangeAsync(tokens);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<List<Token>> BlockTokens(Guid userId)
    {
        ThrowIfDisposed();

        var accessToken = _context.Tokens.FirstOrDefault(x => x.UserId == userId
                                                           && x.TokenType == TokenType.Access
                                                           && x.IsActive);

        var refreshToken = _context.Tokens.FirstOrDefault(x => x.UserId == userId
                                                            && x.TokenType == TokenType.Refresh
                                                            && x.IsActive);
        if (accessToken != null)
        {
            accessToken.IsActive = false;
        }

        if (refreshToken != null)
        {
            refreshToken.IsActive = false;
        }

        await _context.SaveChangesAsync();

        return new List<Token> { accessToken, refreshToken };
    }

    /// <inheritdoc/>
    public async Task<List<Token>> GetTokensByUserId(Guid userId)
    {
        ThrowIfDisposed();
        return await _context.Tokens.Where(x => x.UserId == userId && x.IsActive).ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Token> GetToken(string value)
    {
        ThrowIfDisposed();
        return _context.Tokens.FirstOrDefault(token => token.Value == value);
    }

    /// <inheritdoc/>
    public override async Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        var id = ConvertIdFromString(userId);
        return await _context.Users.Include(x => x.Address)
                                   .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);
    }

    /// <inheritdoc/>
    protected override async Task<User?> FindUserAsync(string userId, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        return await Users.Include(x => x.Address).SingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Protected implementation of Dispose pattern.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }

        base.Dispose();
    }
}
