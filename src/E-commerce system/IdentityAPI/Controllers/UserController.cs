using AutoMapper;
using IdentityAPI.Exceptions;
using IdentityAPI.Models.DataBase.Entities;
using IdentityAPI.Models.ViewModels.Requests;
using IdentityAPI.Models.ViewModels.Responses;
using IdentityAPI.Services;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security;

namespace IdentityAPI.Controllers;

/// <summary>
/// Provides the APIs for handling all the user logic
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    /// <summary>
    /// Object of class <see cref="IUserService"/> providing the APIs for managing user in a persistence store.
    /// </summary>
    private readonly IUserService _userService;

    /// <summary>
    /// Object of class <see cref="IMapper"/> for models mapping
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="UserController"/>.
    /// </summary>
    /// <param name="userService"> Object of class providing the APIs for managing user in a persistence store. </param>
    /// <param name="mapper"> Object of class <see cref="IMapper"/> for models mapping </param>
    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get the user information by Id
    /// </summary>
    /// <param name="userId"> User Id </param>
    /// <returns> The task object containing the action result of getting user information </returns>
    [Authorize]
    [HttpGet("{userId:Guid}")]
    public async Task<ActionResult<UserViewModelResponse>> GetById(Guid userId)
    {
        try
        {
            var user = await _userService.GetById(userId.ToString());
            var response = _mapper.Map<UserViewModelResponse>(user);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        finally
        {
            _userService.Dispose();
        }
    }

    /// <summary>
    /// Get your user information by access token from headers
    /// </summary>
    /// <returns> The task object containing the action result of getting user information </returns>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<UserViewModelResponse>> GetYourUserData()
    {
        try
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Split().Last();
            var userId = new JwtSecurityToken(accessToken).Claims.First(x => x.Type == "Id");
            var user = await _userService.GetById(userId.Value);
            var response = _mapper.Map<UserViewModelResponse>(user);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        finally
        {
            _userService.Dispose();
        }
    }

    /// <summary>
    /// Get new access token with refresh token
    /// </summary>
    /// <param name="model"> Request view model for get access token </param>
    /// <returns> The task object containing the action result of get access token </returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<AccessTokenViewModelResponse>> GetAccessToken(GetAccessTokenRequest model)
    {
        try
        {
            var response = await _userService.GetAccessToken(model.RefreshToken);
            return Ok(response);
        }
        catch (SecurityException ex)
        {
            return BadRequest(ex.Message);
        }
        finally
        {
            _userService.Dispose();
        }
    }

    /// <summary>
    /// Registration of the new user
    /// </summary>
    /// <param name="model"> Registration view model </param>
    /// <returns> The task object containing the authorization result </returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<AuthorizationViewModelResponse>> Register(RegisterViewModel model)
    {
        try
        {
            var user = _mapper.Map<User>(model);
            var result = await _userService.Register(user, model.Password, model.Role);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        finally
        {
            _userService.Dispose();
        }
    }

    /// <summary>
    /// Authorization of the user
    /// </summary>
    /// <param name="model"> Login view model </param>
    /// <returns> The task object containing the authorization result </returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<AuthorizationViewModelResponse>> Login(LoginViewModel model)
    {
        try
        {
            var response = await _userService.Login(model);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (IncorrectPasswordException ex)
        {
            return Unauthorized(ex.Message);
        }
        finally
        {
            _userService.Dispose();
        }
    }

    /// <summary>
    /// Logout from account
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task Logout()
    {
    
    }
}
