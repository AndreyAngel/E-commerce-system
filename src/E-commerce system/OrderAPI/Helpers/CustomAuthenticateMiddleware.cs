using OrderAPI.Services;
using System.Security.Claims;

namespace OrderAPI.Helpers;

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
    public async Task Invoke(HttpContext context)
    {
        var claims = context.User.Identity as ClaimsIdentity;

        if (claims == null)
        {
            context.Items["UserId"] = null;
        }

        var userId = claims.Claims.FirstOrDefault(x => x.Type == "UserId");

        if (userId != null)
        {
            context.Items["UserId"] = userId;
        }

        await _next(context);
    }
}
