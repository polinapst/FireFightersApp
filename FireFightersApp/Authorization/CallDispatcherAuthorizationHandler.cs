using FireFightersApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace FireFightersApp.Authorization
{
    public class CallDispatcherAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, Call>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Call call)
        {
            if (context.User == null || call == null)
            {
                return Task.CompletedTask;
            }

            if (requirement.Name != Constants.AssignedOperationName &&
                requirement.Name != Constants.CompletedOperationName )
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole(Constants.CallDispatcherRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
