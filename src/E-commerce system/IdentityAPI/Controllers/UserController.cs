using AutoMapper;
using IdentityAPI.Exceptions;
using IdentityAPI.Models.DataBase.Entities;
using IdentityAPI.Models.ViewModels;
using IdentityAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
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

    [HttpPost]
    public async Task<ActionResult<IdentityResult>> Register(RegisterViewModel model)
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
    public async Task<ActionResult<IdentityResult>> Login(LoginViewModel model)
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
    [Authorize]
    public async Task Logout()
    {
    
    }

    [Authorize]
    [HttpGet("{userId:Guid}")]
    public ActionResult GetById(Guid userId)
    {
        try
        {
            var user = _userService.GetById(userId);
            return Ok(user);
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
}
