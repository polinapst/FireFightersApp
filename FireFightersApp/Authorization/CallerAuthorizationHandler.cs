using Microsoft.AspNetCore.Identity;
using FireFightersApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace FireFightersApp.Authorization
{
    public class CallerAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, Call>
    {
        UserManager<IdentityUser> _userManager;

        public CallerAuthorizationHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, Call call)
        {
            if (context.User == null || call == null)
            {
                return Task.CompletedTask;
            }

            if (requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName &&
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.DeleteOperationName)
            {
                return Task.CompletedTask;
            }

            if (call.CallerId == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
