using AutoMapper;
using OrderAPI.Exceptions;
using OrderAPI.Models.DataBase.Entities;
using OrderAPI.Models.ViewModels.Requests;
using OrderAPI.Models.ViewModels.Responses;
using OrderAPI.Services;
using OrderAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security;

namespace OrderAPI.Controllers;

/// <summary>
/// Provides the APIs for handling all the user logic
/// </summary>
[Authorize]
[ApiController]
[Route("api/v1/identity/[controller]/[action]")]
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
    /// <response code="200"> Successful completion </response>
    /// <response code="404"> User with this Id wasn't founded </response>
    [Authorize]
    [HttpGet("{userId:Guid}")]
    [ProducesResponseType(typeof(UserViewModelResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
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
    /// <response code="200"> Successful completion </response>
    /// <response code="404"> User with this Id from the access token wasn't founded </response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(UserViewModelResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
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
    /// <param name="model"> Model of request for get access token </param>
    /// <returns> The task object containing the action result of get access token </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="403"> Insecure request </response>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AccessTokenViewModelResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<ActionResult<AccessTokenViewModelResponse>> GetAccessToken(GetAccessTokenRequest model)
    {
        try
        {
            var response = await _userService.GetAccessToken(model.RefreshToken);
            return Ok(response);
        }
        catch (SecurityException ex)
        {
            return Forbid(ex.Message);
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
    /// <response code="201"> User registrated </response>
    /// <response code="400"> Incorrect data was sent during registration </response>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthorizationViewModelResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(IdentityErrorsViewModelResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<AuthorizationViewModelResponse>> Register(RegisterViewModel model)
    {
        try
        {
            var user = _mapper.Map<User>(model);
            var result = await _userService.Register(user, model.Password, model.Role);

            if (result.GetType() == typeof(IdentityErrorsViewModelResponse))
            {
                return BadRequest(result);
            }

            return Created(new Uri($"https://localhost:7281/api/v1/identity/User/GetById/{user.Id}"), result);
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
    /// <response code="200"> Successful completion </response>
    /// <response code="404"> Incorrect data was sent during registration </response>
    /// <response code="401"> Incorrect password </response>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthorizationViewModelResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
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
