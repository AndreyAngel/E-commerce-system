using IdentityAPI.Exceptions;
using IdentityAPI.Helpers;
using IdentityAPI.Models.DTO;
using Infrastructure.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using MassTransit;
using IdentityAPI.Models.Enums;
using Infrastructure.Exceptions;
using Infrastructure;
using IdentityAPI.DataBase.Entities;
using IdentityAPI.DataBase;

namespace IdentityAPI.Services;

/// <summary>
/// Provides the APIs for managing user in a persistence store.
/// </summary>
public class UserService : UserManager<User>, IUserService
{
    /// <summary>
    /// Configurations of application
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// <see cref="IBusControl"/>.
    /// </summary>
    private readonly IBusControl _bus;

    /// <inheritdoc/>
    public new ICustomUserStore Store { get; init;  }

    /// <summary>
    /// True, if object is disposed
    /// False, if object isn't disposed
    /// </summary>
    private bool _disposed = false;

    /// <summary>
    /// Constructs a new instance of <see cref="UserManager{TUser}"/>.
    /// </summary>
    /// <param name="configuration"> Configurations of application </param>
    /// <param name="bus"></param>
    /// <param name="store">The persistence store the manager will operate over.</param>
    /// <param name="optionsAccessor">The accessor used to access the <see cref="IdentityOptions"/>.</param>
    /// <param name="passwordHasher">The password hashing implementation to use when saving passwords.</param>
    /// <param name="userValidators">A collection of <see cref="IUserValidator{TUser}"/> to validate users against.</param>
    /// <param name="passwordValidators">A collection of <see cref="IPasswordValidator{TUser}"/> to validate passwords against.</param>
    /// <param name="keyNormalizer">The <see cref="ILookupNormalizer"/> to use when generating index keys for users.</param>
    /// <param name="errors">The <see cref="IdentityErrorDescriber"/> used to provider error messages.</param>
    /// <param name="services">The <see cref="IServiceProvider"/> used to resolve services.</param>
    /// <param name="logger">The logger used to log messages, warnings and errors.</param>
    public UserService( IConfiguration configuration,
                        IBusControl bus,
                        ICustomUserStore store,
                        IOptions<IdentityOptions> optionsAccessor,
                        IPasswordHasher<User> passwordHasher,
                        IEnumerable<IUserValidator<User>> userValidators,
                        IEnumerable<IPasswordValidator<User>> passwordValidators,
                        ILookupNormalizer keyNormalizer,
                        IdentityErrorDescriber errors,
                        IServiceProvider services,
                        ILogger<UserManager<User>> logger) : base(store,
                                                                  optionsAccessor,
                                                                  passwordHasher,
                                                                  userValidators,
                                                                  passwordValidators,
                                                                  keyNormalizer,
                                                                  errors,
                                                                  services,
                                                                  logger)
    {
        _configuration = configuration;
        _bus = bus;
        Store = store;
    }

    /// <inheritdoc/>
    public async Task<User> GetById(Guid id)
    {
        ThrowIfDisposed();
        var user = await FindByIdAsync(id.ToString());

        if (user == null)
        {
            throw new NotFoundException("User with this Id wasn't founded", nameof(id));
        }

        return user;
    }

    /// <inheritdoc/>
    public async Task<AuthorizationDTOResponse> Login(LoginDTORequest model)
    {
        ThrowIfDisposed();
        var user = await FindByEmailAsync(model.Email);

        if (user == null)
        {
            throw new NotFoundException("User with this Email wasn't founded", nameof(model.Email));
        }

        return await Login(user, model.Password);
    }

    /// <inheritdoc/>
    public async Task<IIdentityDTOResponse> Register(User user, string password, Role role)
    {
        ThrowIfDisposed();
        var userRole = new IdentityRole { Name = Enum.GetName(typeof(Role), role) };
        var result = await CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return new IdentityErrorsDTOResponse(result.Errors);
        }

        if (userRole.Name == Role.Buyer.ToString())
        {
            await CreateCart(new Guid(user.Id));
        }
        
        await AddToRoleAsync(user, userRole.Name);

        var claims = new List<Claim>()
        {
            new Claim("UserId", user.Id),
            new Claim(ClaimTypes.Role, userRole.Name)
        };

        await AddClaimsAsync(user, claims);

        return await GenerateTokens(new Guid(user.Id), userRole.Name, claims);
    }

    /// <inheritdoc/>
    public async Task<AuthorizationDTOResponse> GetAccessToken(string refreshToken)
    {
        ThrowIfDisposed();
        var validatedToken = JwtTokenHelper.ValidateToken(_configuration, refreshToken);
        var userId = new JwtSecurityToken(validatedToken).Claims.ToList().FirstOrDefault(x => x.Type == "UserId");
        

        if (userId == null)
        {
            throw new SecurityException("Incorrect refreshToken");
        }

        var user = await FindByIdAsync(userId.Value);

        if (user == null)
        {
            throw new SecurityException("Incorrect refreshToken");
        }

        var roles = await GetRolesAsync(user);

        if (roles.Count == 0)
        {
            throw new SecurityException("Incorrect refreshToken");
        }

        return await GenerateTokens(new Guid(userId.Value), roles[0]);
    }

    /// <inheritdoc/>
    public void Logout(Guid userId)
    {
        ThrowIfDisposed();
        Store.BlockTokens(userId);
    }

    /// <inheritdoc/>
    public async Task<bool> TokensIsActive(Guid userId)
    {
        ThrowIfDisposed();
        var tokens = await Store.GetTokensByUserId(userId);

        if (tokens.Count == 0)
        {
            return false;
        }

        return (tokens[0].IsActive && tokens[1].IsActive);
    }

    /// <inheritdoc/>
    public async Task<IdentityErrorsDTOResponse?> Update(User user, Guid userId)
    {
        ThrowIfDisposed();
        var res = await FindByIdAsync(userId.ToString());

        if (res == null)
        {
            throw new NotFoundException("User with this Id wasn't founded", nameof(userId));
        }

        res.Name = user.Name ?? res.Name;
        res.Surname = user.Surname ?? res.Surname;
        res.BirthDate = user.BirthDate ?? res.BirthDate;
        res.Address = user.Address ?? res.Address;

        var result = await UpdateAsync(res);

        if (!result.Succeeded)
        {
            return new IdentityErrorsDTOResponse(result.Errors);
        }

        return null;
    }

    /// <inheritdoc/>
    private async Task CreateCart(Guid userId)
    {
        ThrowIfDisposed();
        await RabbitMQClient.Request<CartDTORabbitMQ>(_bus, new(userId), new("rabbitmq://localhost/createCartQueue"));
    }

    /// <inheritdoc/>
    private async Task<AuthorizationDTOResponse> Login(User user , string password)
    {
        ThrowIfDisposed();
        if (!await CheckPasswordAsync(user, password))
        {
            throw new IncorrectPasswordException("Incorrect password", nameof(password));
        }

        var roles = await GetRolesAsync(user);

        return await GenerateTokens(new Guid(user.Id), roles[0]);
    }

    /// <inheritdoc/>
    private async Task<AuthorizationDTOResponse> GenerateTokens(Guid userId, string roleName, List<Claim>? claims = null)
    {
        ThrowIfDisposed();
        if (claims == null)
        {
            claims = new List<Claim>()
            {
                new Claim("UserId", userId.ToString()),
                new Claim(ClaimTypes.Role, roleName)
            };
        }

        var refreshToken = JwtTokenHelper.GenerateJwtRefreshToken(_configuration, new List<Claim>() { claims[0] });
        var accessToken = JwtTokenHelper.GenerateJwtAccessToken(_configuration, claims);

        await Store.BlockTokens(userId);
        await Store.AddRangeTokenAsync(new List<Token>()
        {
            new Token()
            {
                UserId = userId,
                TokenType = TokenType.Access,
                Value = accessToken
            },

            new Token()
            {
                UserId = userId,
                TokenType = TokenType.Refresh,
                Value = refreshToken
            }
        });

        return new AuthorizationDTOResponse(900, accessToken, refreshToken, "Bearer", userId);
    }

    /// <inheritdoc/>
    protected new void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            _disposed = true;
        }

        base.Dispose(disposing);
    }
}