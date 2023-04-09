using IdentityAPI.Services;
using System.Security.Claims;

namespace IdentityAPI.Helpers;

/// <summary>
/// Middleware adding user in context items
/// </summary>
public class CustomAuthenticateMiddleware
{
    /// <summary>
    /// Request delegate
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of <see cref="CustomAuthenticateMiddleware"/>.
    /// </summary>
    /// <param name="next">The next item in the middleware pipeline.</param>
    public CustomAuthenticateMiddleware(RequestDelegate next)
    {
        if (next == null)
        {
            throw new ArgumentNullException(nameof(next));
        }

        _next = next;
    }

    /// <summary>
    /// Invokes the middleware performing authentication.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/>.</param>
    /// <param name="userService">The <see cref="IUserService"/>.</param>
    public async Task Invoke(HttpContext context, IUserService userService)
    {
        try
        {
            var claims = context.User.Identity as ClaimsIdentity;
            var userId = claims.Claims.FirstOrDefault(x => x.Type == "UserId");
            var accessToken = context.Request.Headers["Authorization"].ToString().Split().Last();

            if (userId != null && await userService.TokenIsActive(accessToken))
            {
                var user = await userService.GetById(new Guid(userId.Value));
                context.Items["User"] = user;
            }
        }
        catch (ArgumentNullException)
        {
            context.Items["User"] = null;
        }
        catch (NullReferenceException)
        {
            context.Items["User"] = null;
        }

        await _next(context);
    }
}
