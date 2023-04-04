using IdentityAPI.Models.DataBase.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models.DataBase;
using OrderAPI.Models.DataBase.Entities;
using OrderAPI.Models.Enums;

namespace IdentityAPI.Models.DataBase;

public class CustomUserStore : UserStore<User>, ICustomUserStore
{
    /// <summary>
    /// Database context
    /// </summary>
    private readonly Context _context;

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

    /// <inheritdoc/>
    public async Task AddRangeTokenAsync(IEnumerable<Token> tokens)
    {
        await _context.Tokens.AddRangeAsync(tokens);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task BlockTokens(Guid userId)
    {
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
    }

    /// <inheritdoc/>
    public async Task<List<Token>> GetTokensByUserId(Guid userId)
    {
        return await _context.Tokens.Where(x => x.UserId == userId && x.IsActive).ToListAsync();
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
        return await Users.Include(x => x.Address).SingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);
    }
}
