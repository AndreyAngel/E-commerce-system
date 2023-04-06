﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using IdentityAPI.Models.DataBase.Entities;

namespace IdentityAPI.Helpers;

/// <summary>
/// Specifies that the class or method that this attribute is applied to don't requires the specified authorization
/// </summary>
public class LoginAttribute : Attribute
{
    /// <inheritdoc/>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (User?)context.HttpContext.Items["User"];

        if (user != null)
        {
            context.Result = new JsonResult(new { message = "Already authorized" })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
}
