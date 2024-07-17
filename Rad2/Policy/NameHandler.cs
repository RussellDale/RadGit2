using System.Security.Claims;
using DocumentFormat.OpenXml.Wordprocessing;
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
        public Task HandleRequirementAsync3(IList<Claim> claims, NameRequirement requirement)
        {
            Claim? name = null;

            name = Handle2(claims, requirement, name);

            return Task.CompletedTask;
        }
        private Claim? Handle(ClaimsPrincipal user, NameRequirement requirement, Claim? name)
        {
            name = user.FindFirst(c => c.Type == "Name");

            return Claim(name, requirement);
        }
        private Claim? Handle2(IList<Claim> claims, NameRequirement requirement, Claim? name)
        {
            if (claims.Count == 0) { }
            else
                name = claims.First(c => c.Type == "Name");

            return Claim(name, requirement);
        }

        private Claim? Claim(Claim? name, NameRequirement requirement)
        {
            if (name is not null)
            {
                requirement.Name = name.Value;
            }

            return name;
        }
    }
}
