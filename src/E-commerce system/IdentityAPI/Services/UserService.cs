using IdentityAPI.Exceptions;
using IdentityAPI.Helpers;
using IdentityAPI.Models.DataBase.Entities;
using IdentityAPI.Models.ViewModels.Requests;
using IdentityAPI.Models.ViewModels.Responses;
using Infrastructure.Exceptions;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;

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
    /// Constructs a new instance of <see cref="UserManager{TUser}"/>.
    /// </summary>
    /// <param name="configuration"> Configurations of application </param>
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
                        IUserStore<User> store,
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
    }

    /// <summary>
    /// Gets the user by Id
    /// </summary>
    /// <param name="id"> The user Id </param>
    /// <returns> The task object containing the action result of getting user information </returns>
    /// <exception cref="NotFoundException"> User with this Id wasn't founded </exception>
    public async Task<User> GetById(string id)
    {
        var user = await FindByIdAsync(id.ToString());

        if (user == null)
        {
            throw new NotFoundException("User with this Id wasn't founded", nameof(id));
        }

        return user;
    }

    /// <summary>
    /// Authorization of the user
    /// </summary>
    /// <param name="model"> Login view model </param>
    /// <returns> The task object containing the authorization result </returns>
    /// <exception cref="NotFoundException"> User with this Email wasn't founded </exception>
    /// <exception cref="IncorrectPasswordException"> Incorrect password </exception>
    public async Task<AuthorizationViewModelResponse> Login(LoginViewModel model)
    {
        var user = await FindByEmailAsync(model.Email);

        if (user == null)
        {
            throw new NotFoundException("User with this Email wasn't founded", nameof(model.Email));
        }

        var hashPassword = PasswordHasher.HashPassword(user, model.Password);

        if (await CheckPasswordAsync(user, hashPassword))
        {
            throw new IncorrectPasswordException("Incorrect password", nameof(model.Password));
        }

        var roles = await GetRolesAsync(user);

        var claims = new List<Claim>()
        {
            new Claim("Id", user.Id),
        };

        var refraeshToken = JwtTokenHelper.GenerateJwtRefreshToken(_configuration, claims);

        claims.Add(new Claim(ClaimTypes.Role, roles[0]));

        var accessToken = JwtTokenHelper.GenerateJwtAccessToken(_configuration, claims);

        return new AuthorizationViewModelResponse(900, accessToken, refraeshToken);
    }

    /// <summary>
    /// Registration of the new user
    /// </summary>
    /// <param name="user"> Object of the user </param>
    /// <param name="Password"> User password </param>
    /// <param name="role"> User role </param>
    /// <returns> The task object containing the authorization result </returns>
    public async Task<IIdentityViewModelResponse> Register(User user, string Password, Role role)
    {
        var userRole = new IdentityRole { Name = Enum.GetName(typeof(Role), role) };
        var result = await CreateAsync(user, Password);

        if (!result.Succeeded)
        {
            return new IdentityErrorsViewModelResponse(result.Errors);
        }

        var claims = new List<Claim>()
        {
            new Claim("UserId", user.Id)
        };

        var refreshToken = JwtTokenHelper.GenerateJwtRefreshToken(_configuration, claims);

        await AddToRoleAsync(user, userRole.Name);
        await AddClaimsAsync(user, claims);

        claims.Add(new Claim(ClaimTypes.Role, userRole.Name));
        
        var accessToken = JwtTokenHelper.GenerateJwtAccessToken(_configuration, claims);

        return new AuthorizationViewModelResponse(900, accessToken, refreshToken);
    }

    /// <summary>
    /// Get new access token with refresh token
    /// </summary>
    /// <param name="refreshToken"> refresh token </param>
    /// <returns> The task object containing the action result of get access token </returns>
    /// <exception cref="SecurityException"> Incorrect refreshToken </exception>
    public async Task<AccessTokenViewModelResponse> GetAccessToken(string refreshToken)
    {
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

        var claims = new List<Claim>()
        {
            new Claim("UserId", userId.Value),
            new Claim(ClaimTypes.Role, roles[0])
        };

        var accessToken = JwtTokenHelper.GenerateJwtAccessToken(_configuration, claims);

        return new AccessTokenViewModelResponse(900, accessToken);
    }

    /// <summary>
    /// Logout from account
    /// </summary>
    /// <returns> Task </returns>
    public async Task Logout()
    {
        throw new NotImplementedException();
    }
}