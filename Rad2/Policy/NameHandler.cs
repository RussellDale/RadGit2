using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace Rad2.Policy
{
    public class NameHandler : AuthorizationHandler<NameRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NameRequirement requirement)
        {
            Claim? name = null;

            name = Handle(context.User, requirement, name);

            if (name is null)
                context.Fail();
            else
                context.Succeed(requirement);

            return Task.CompletedTask;
        }

        public Task HandleRequirementAsync2(AuthenticationState context, NameRequirement requirement)
        {
            Claim? name = null;

            name = Handle(context.User, requirement, name);

            return Task.CompletedTask;
        }

        private Claim? Handle(ClaimsPrincipal user, NameRequirement requirement, Claim? name)
        {
            name = user.FindFirst(c => c.Type == "Name");

            if (name is not null)
            {
                requirement.Name = name.Value;
            }

            return name;
        }
    }
}
