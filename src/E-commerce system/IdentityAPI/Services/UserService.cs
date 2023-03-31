using IdentityAPI.Exceptions;
using IdentityAPI.Models.DataBase.Entities;
using IdentityAPI.Models.Enums;
using IdentityAPI.Models.ViewModels;
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
        var token = _configuration.GenerateJwtToken(user, roles[0]);

        return new AuthenticateViewModelResponse(user, token, roles[0]);
    }

    public async Task<AuthenticateViewModelResponse> Register(User user, string Password, Role role)
    {
        var userRole = new IdentityRole { Name = Enum.GetName(typeof(Role), role) };
        var result = await _userManager.CreateAsync(user, Password);

        if (!result.Succeeded)
        {
            return new AuthenticateViewModelResponse(result.Errors.ToList());
        }

        await _userManager.AddToRoleAsync(user, userRole.Name);
        await _userManager.AddClaimsAsync(user, new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(ClaimTypes.Role, userRole.Name)
        });

        var token = _configuration.GenerateJwtToken(user, userRole.Name);

        return new AuthenticateViewModelResponse(user, token, userRole.Name);
    }

    public async Task Logout()
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetById(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
        {
            throw new NotFoundException("User with this Id wasn't founded", nameof(id));
        }

        return user;
    }
}