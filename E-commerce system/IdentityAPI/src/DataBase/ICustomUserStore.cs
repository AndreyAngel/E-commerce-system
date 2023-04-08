using IdentityAPI.DataBase.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.DataBase;

/// <summary>
/// Provides an abstraction for a store which manages user accounts
/// </summary>
public interface ICustomUserStore : IUserStore<User>
{
    /// <summary>
    /// Gets database context
    /// </summary>
    public Context Context { get; }

    /// <summary>
    /// Adds tokens <see cref="IEnumerable{Token}"/> in database context
    /// </summary>
    /// <param name="tokens"> Tokens </param>
    /// <returns> Task object </returns>
    public Task AddRangeTokenAsync(IEnumerable<Token> tokens);

    /// <summary>
    /// Blocks tokens: Field "IsActive" = false
    /// </summary>
    /// <param name="userId"> User Id </param>
    /// <returns> Task object </returns>
    public Task BlockTokens(Guid userId);

    /// <summary>
    /// Gets access token and refresh token by user Id
    /// </summary>
    /// <param name="userId"> User Id </param>
    /// <returns> Task object </returns>
    public Task<List<Token>> GetTokensByUserId(Guid userId);
}
