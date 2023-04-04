using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrderAPI.Models.DataBase.Entities;

namespace IdentityAPI.Helpers;

/// <summary>
/// Specifies that the class or method that this attribute is applied to requires the specified authorization.
/// </summary>
public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    /// <inheritdoc/>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (User?)context.HttpContext.Items["User"];

        if (user == null)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}
