﻿using AutoMapper;
using IdentityAPI.Exceptions;
using IdentityAPI.Models.DTO;
using IdentityAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security;
using IdentityAPI.Helpers;
using Infrastructure.Exceptions;
using IdentityAPI.DataBase.Entities;

namespace IdentityAPI.Controllers;

/// <summary>
/// Provides the APIs for handling all the user logic
/// </summary>
[ApiController]
[Route("api/v1/IdentityAPI/[controller]/[action]")]
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
    /// <response code="401"> Unauthorized </response>
    [HttpGet("{userId:Guid}")]
    [Authorize(Policy = "Public")]
    [ProducesResponseType(typeof(UserDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(Guid userId)
    {
        try
        {
            var user = await _userService.GetById(userId);
            var response = _mapper.Map<UserDTOResponse>(user);

            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
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
    [ProducesResponseType(typeof(AuthorizationDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> GetAccessToken(GetAccessTokenDTORequest model)
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
    }

    /// <summary>
    /// Registration of the new user
    /// </summary>
    /// <param name="model"> Registration DTO </param>
    /// <returns> The task object containing the authorization result </returns>
    /// <response code="201"> User registrated </response>
    /// <response code="400"> Incorrect data was sent during registration </response>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthorizationDTOResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(IdentityErrorsDTOResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Register(RegisterDTORequest model)
    {
        var user = _mapper.Map<User>(model);
        var result = await _userService.Register(user, model.Password, model.Role);

        if (result.GetType() == typeof(IdentityErrorsDTOResponse))
        {
            return BadRequest(result);
        }

        await _userService.Store.Context.SaveChangesAsync();

        return Created(new Uri($"https://localhost:7281/api/v1/identity/User/GetById/{user.Id}"), result);
    }

    /// <summary>
    /// Authorization of the user
    /// </summary>
    /// <param name="model"> Login DTO </param>
    /// <returns> The task object containing the authorization result </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="404"> Incorrect data was sent during authorization </response>
    /// <response code="401"> Incorrect password </response>
    /// <response code="403"> Already authorized </response>
    [Login]
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthorizationDTOResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(JsonResult), (int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> Login(LoginDTORequest model)
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
    }

    /// <summary>
    /// Logout from account
    /// </summary>
    /// <returns> The task object </returns>
    /// <response code="200"> Successful completion </response>
    /// <response code="401"> Unauthorized </response>
    [HttpPost]
    [Authorize(Policy = "Public")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public IActionResult Logout()
    {
        var user = HttpContext.Items["User"] as User;
        _userService.Logout(new Guid(user.Id));
        return Ok();
    }

    ///<summary>
    /// Update of user date
    /// </summary>
    /// <param name="model"> User data DTO </param>
    /// <param name="userId"> User Id </param>
    /// <returns> Task object contaning request result </returns>
    /// <response code="204"> Successful completion </response>
    /// <response code="400"> Bad request </response>
    /// <response code="401"> Unauthorized </response>
    [HttpPatch("{userId:Guid}")]
    [Authorize(Policy = "Public")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(IdentityErrorsDTOResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(UserDTORequest model, Guid userId)
    {
        var user = _mapper.Map<User>(model);
        var result = await _userService.Update(user, userId);

        if (result != null)
        {
            return BadRequest(result);
        }

        return NoContent();
    }
}
