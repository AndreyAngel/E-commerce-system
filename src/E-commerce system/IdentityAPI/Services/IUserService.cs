using IdentityAPI.Models.DataBase.Entities;
using IdentityAPI.Models.Enums;
using IdentityAPI.Models.ViewModels;

namespace IdentityAPI.Services;

public interface IUserService
{
    Task<AuthenticateViewModelResponse> Register(User user, string Password, Role role);

    Task<AuthenticateViewModelResponse> Login(LoginViewModel model);

    Task Logout();

    Task<User> GetById(Guid id);
}