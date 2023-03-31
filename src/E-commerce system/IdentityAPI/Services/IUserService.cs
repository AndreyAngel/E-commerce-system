using IdentityAPI.Models.DataBase.Entities;
using IdentityAPI.Models.Enums;
using IdentityAPI.Models.ViewModels.Requests;
using IdentityAPI.Models.ViewModels.Responses;

namespace IdentityAPI.Services;

public interface IUserService
{
    Task<User> GetById(string id);

    AccessTokenViewModelResponse GetAccessToken(string refreshToken);

    Task<IIdentityViewModelResponse> Register(User user, string Password, Role role);

    Task<AuthenticateViewModelResponse> Login(LoginViewModel model);

    Task Logout();
}