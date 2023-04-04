using OrderAPI.Models.DataBase.Entities;
using OrderAPI.Models.ViewModels.Requests;
using OrderAPI.Models.ViewModels.Responses;
using OrderAPI.Exceptions;
using System.Security;
using OrderAPI.Models.Enums;
using IdentityAPI.Models.DataBase;

namespace OrderAPI.Services;

/// <summary>
/// Interface for class providing the APIs for managing user in a persistence store.
/// </summary>
public interface IUserService : IDisposable
{
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
    Task<AuthorizationViewModelResponse> GetAccessToken(string refreshToken);

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
    void Logout(Guid userId);

    /// <summary>
    /// Checks tokens
    /// If true, token is active
    /// If false, token isn't active
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> TokensIsActive(Guid userId);

    /// <summary>
    /// Update of user date
    /// </summary>
    /// <param name="user"> User </param>
    /// <param name="userId"> User Id </param>
    /// <returns> Task object </returns>
    /// <exception cref="NotFoundException"> User with this Id wasn't founded </exception>
    Task<IdentityErrorsViewModelResponse?> Update(User user, Guid userId);
}