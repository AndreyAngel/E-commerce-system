using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;

namespace OrderAPI.Helpers;

public class AuthorizeHandler : AuthorizationHandler<RolesAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                    RolesAuthorizationRequirement requirement)
    {
        if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
        {
            return Task.CompletedTask;
        }

        var role = context.User.FindFirst(c => c.Type == ClaimTypes.Role);

        if (requirement.AllowedRoles.ToList().Select(x => x.ToLower()).Contains(role.Value.ToLower()))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
