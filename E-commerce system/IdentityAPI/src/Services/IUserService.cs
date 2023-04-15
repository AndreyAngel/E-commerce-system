using IdentityAPI.Exceptions;
using System.Security;
using IdentityAPI.Models.Enums;
using Infrastructure.Exceptions;
using IdentityAPI.DataBase.Entities;
using IdentityAPI.DataBase;
using IdentityAPI.Models.DTO.Response;
using IdentityAPI.Models.DTO.Requests;

namespace IdentityAPI.Services;

/// <summary>
/// Interface for class providing the APIs for managing user in a persistence store.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Represents a new instance of a persistence store for users, using the default implementation
    /// </summary>
    public ICustomUserStore Store { get; }

    /// <summary>
    /// Gets the user by Id
    /// </summary>
    /// <param name="id"> The user Id </param>
    /// <returns> The task object containing the action result of getting user information </returns>
    /// <exception cref="NotFoundException"> User with this Id wasn't founded </exception>
    Task<User> GetById(Guid id);

    /// <summary>
    /// Get new access token with refresh token
    /// </summary>
    /// <param name="refreshToken"> refresh token </param>
    /// <returns> The task object containing the action result of get access token </returns>
    /// <exception cref="SecurityException"> Incorrect refreshToken </exception>
    Task<AuthorizationDTOResponse> GetAccessToken(string refreshToken);

    /// <summary>
    /// Registration of the new user
    /// </summary>
    /// <param name="user"> Object of the user </param>
    /// <param name="Password"> User password </param>
    /// <param name="role"> User role </param>
    /// <returns> The task object containing the authorization result </returns>
    Task<IDTOResponse> Register(User user, string Password, Role role);

    /// <summary>
    /// Authorization of the user
    /// </summary>
    /// <param name="model"> Login view model </param>
    /// <returns> The task object containing the authorization result </returns>
    /// <exception cref="NotFoundException"> User with this Email wasn't founded </exception>
    /// <exception cref="IncorrectPasswordException"> Incorrect password </exception>
    Task<AuthorizationDTOResponse> Login(LoginDTORequest model);

    /// <summary>
    /// Logout from account
    /// </summary>
    /// <returns> Task object </returns>
    Task Logout(Guid userId);

    /// <summary>
    /// Checks token
    /// If true, token is active
    /// If false, token isn't active
    /// </summary>
    /// <param name="token"></param>
    /// <returns> true if token is active, false if token isn't active </returns>
    Task<bool> TokenIsActive(string token);

    /// <summary>
    /// Update of user date
    /// </summary>
    /// <param name="user"> User </param>
    /// <param name="userId"> User Id </param>
    /// <returns> Task object containing result of updating user data </returns>
    /// <exception cref="NotFoundException"> User with this Id wasn't founded </exception>
    Task<IDTOResponse?> Update(User user, Guid userId);

    /// <summary>
    /// Change password
    /// </summary>
    /// <param name="email"> User Email </param>
    /// <param name="oldPassword"> Old password </param>
    /// <param name="newPassword"> New password </param>
    /// <returns> Task object containing result of changing password </returns>
    Task<IDTOResponse?> ChangePassword(string email, string oldPassword, string newPassword);
}