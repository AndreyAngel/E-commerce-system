using Microsoft.EntityFrameworkCore;

namespace GatewayAPI.DataBase;

/// <summary>
/// Class for the Entity Framework database context
/// </summary>
public class Context : DbContext
{
    /// <summary>
    /// Creates an instance of the <see cref="Context"/>.
    /// </summary>
    /// <param name="options"> <see cref="DbContextOptions{Context}"/> </param>
    public Context(DbContextOptions<Context> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Token> Tokens { get; set; }
}
