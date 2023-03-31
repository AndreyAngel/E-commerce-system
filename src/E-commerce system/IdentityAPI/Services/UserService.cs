using IdentityAPI.Exceptions;
using IdentityAPI.Helpers;
using IdentityAPI.Models.DataBase.Entities;
using IdentityAPI.Models.Enums;
using IdentityAPI.Models.ViewModels.Requests;
using IdentityAPI.Models.ViewModels.Responses;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityAPI.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public UserService(UserManager<User> userManager,  IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<User> GetById(string id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            throw new NotFoundException("User with this Id wasn't founded", nameof(id));
        }

        return user;
    }

    public async Task<AuthenticateViewModelResponse> Login(LoginViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            throw new NotFoundException("User with this Email wasn't founded", nameof(model.Email));
        }

        var hashPassword = _userManager.PasswordHasher.HashPassword(user, model.Password);
        if (await _userManager.CheckPasswordAsync(user, hashPassword))
        {
            throw new IncorrectPasswordException("Incorrect password", nameof(model.Password));
        }

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>()
        {
            new Claim("Id", user.Id),
            new Claim(ClaimTypes.Role, roles[0])
        };

        var refraeshToken = JwtTokenHelper.GenerateJwtRefreshToken(_configuration, claims);
        var accessToken = JwtTokenHelper.GenerateJwtAccessToken(_configuration, claims);

        return new AuthenticateViewModelResponse(900, accessToken, refraeshToken);
    }

    public async Task<IIdentityViewModelResponse> Register(User user, string Password, Role role)
    {
        var userRole = new IdentityRole { Name = Enum.GetName(typeof(Role), role) };
        var result = await _userManager.CreateAsync(user, Password);

        if (!result.Succeeded)
        {
            return new IdentityErrorsViewModelResponse(result.Errors);
        }

        var claims = new List<Claim>()
        {
            new Claim("Id", user.Id),
            new Claim(ClaimTypes.Role, userRole.Name)
        };

        await _userManager.AddToRoleAsync(user, userRole.Name);
        await _userManager.AddClaimsAsync(user, claims);

        var refreshToken = JwtTokenHelper.GenerateJwtRefreshToken(_configuration, claims);
        var accessToken = JwtTokenHelper.GenerateJwtAccessToken(_configuration, claims);

        return new AuthenticateViewModelResponse(900, accessToken, refreshToken);
    }

    public AccessTokenViewModelResponse GetAccessToken(string refreshToken)
    {
        var validatedToken = JwtTokenHelper.ValidateToken(_configuration, refreshToken);
        var claims = new JwtSecurityToken(validatedToken).Claims.ToList();
        var accessToken = JwtTokenHelper.GenerateJwtAccessToken(_configuration, claims);

        return new AccessTokenViewModelResponse(900, accessToken);
    }

    public async Task Logout()
    {
        throw new NotImplementedException();
    }
}