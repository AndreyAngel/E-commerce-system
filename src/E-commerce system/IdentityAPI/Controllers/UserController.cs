using AutoMapper;
using IdentityAPI.Exceptions;
using IdentityAPI.Models.DataBase.Entities;
using IdentityAPI.Models.ViewModels.Requests;
using IdentityAPI.Models.ViewModels.Responses;
using IdentityAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityAPI.Controllers;


[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    private readonly UserManager<User> _userManager;

    private readonly IMapper _mapper;

    public UserController(IUserService userService, UserManager<User> userManager, IMapper mapper)
    {
        _userService = userService;
        _userManager = userManager;
        _mapper = mapper;
    }

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
            _userManager.Dispose();
        }
    }

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
            _userManager.Dispose();
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult<AccessTokenViewModelResponse> GetAccessToken(GetAccessTokenRequest model)
    {
        try
        {
            var response = _userService.GetAccessToken(model.RefreshToken);
            return Ok(response);
        }
        finally
        {
            _userManager.Dispose();
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticateViewModelResponse>> Register(RegisterViewModel model)
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
            _userManager.Dispose();
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticateViewModelResponse>> Login(LoginViewModel model)
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
            _userManager.Dispose();
        }
    }

    [HttpPost]
    public async Task Logout()
    {
    
    }
}
