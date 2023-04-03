using OrderAPI.Models.DataBase.Entities;
using OrderAPI.Models.ViewModels.Requests;
using OrderAPI.Models.ViewModels.Responses;
using OrderAPI.Models;
using OrderAPI.Exceptions;
using OrderAPI.Exceptions;
using System.Security;
using OrderAPI.Models.Enums;

namespace OrderAPI.Services;

/// <summary>
/// Interface for class providing the APIs for managing user in a persistence store.
/// </summary>
public interface IUserService : IDisposable
{
    /// <summary>
    /// Gets the user by Id
    /// </summary>
    /// <param name="id"> The user Id </param>
    /// <returns> The task object containing the action result of getting user information </returns>
    /// <exception cref="NotFoundException"> User with this Id wasn't founded </exception>
    Task<User> GetById(string id);

    /// <summary>
    /// Get new access token with refresh token
    /// </summary>
    /// <param name="refreshToken"> refresh token </param>
    /// <returns> The task object containing the action result of get access token </returns>
    /// <exception cref="SecurityException"> Incorrect refreshToken </exception>
    Task<AccessTokenViewModelResponse> GetAccessToken(string refreshToken);

    /// <summary>
    /// Registration of the new user
    /// </summary>
    /// <param name="user"> Object of the user </param>
    /// <param name="Password"> User password </param>
    /// <param name="role"> User role </param>
    /// <returns> The task object containing the authorization result </returns>
    Task<IIdentityViewModelResponse> Register(User user, string Password, Role role);

    /// <summary>
    /// Authorization of the user
    /// </summary>
    /// <param name="model"> Login view model </param>
    /// <returns> The task object containing the authorization result </returns>
    /// <exception cref="NotFoundException"> User with this Email wasn't founded </exception>
    /// <exception cref="IncorrectPasswordException"> Incorrect password </exception>
    Task<AuthorizationViewModelResponse> Login(LoginViewModel model);

    /// <summary>
    /// Logout from account
    /// </summary>
    /// <returns> Task </returns>
    Task Logout();
}