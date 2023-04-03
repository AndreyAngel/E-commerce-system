using IdentityAPI.Models.DataBase.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityAPI.Models.DataBase;

/// <summary>
/// Class for the Entity Framework database context used for identity
/// </summary>
public class Context : IdentityDbContext<User>
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
