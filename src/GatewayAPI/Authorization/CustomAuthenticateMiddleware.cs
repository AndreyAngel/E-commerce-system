using GatewayAPI.Authorization;

namespace GatewayAPI.Helpers;

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
    /// <param name="tokenService">The <see cref="ITokenService"/>.</param>
    public async Task Invoke(HttpContext context, ITokenService tokenService)
    {

        var acccessToken = context.Request.Headers["Authorization"].ToString().Split().Last();

        if (string.IsNullOrEmpty(acccessToken) || !tokenService.IsActive(acccessToken))
        {
            context.Request.Headers["Authorization"] = "";
        }

        await _next(context);
    }
}
