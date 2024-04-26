using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace Rad2.Policy
{
    public class CrudpHandler : AuthorizationHandler<CrudpRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CrudpRequirement requirement)
        {
            Claim? crudp = null;

            crudp = Handle(context.User, requirement, crudp);

            if (crudp is null)
                context.Fail();
            else
                context.Succeed(requirement);

            return Task.CompletedTask;
        }

        public Task HandleRequirementAsync2(AuthenticationState context, CrudpRequirement requirement)
        {
            Claim? crudp = null;

            crudp = Handle(context.User, requirement, crudp);

            return Task.CompletedTask;
        }

        private Claim? Handle(ClaimsPrincipal user, CrudpRequirement requirement, Claim? crudp)
        {
            crudp = user.FindFirst(c => c.Type == "CRUDP");

            if (crudp is not null)
            {
                if (crudp.Value.Substring(0, 1) == "0") requirement.Create = false;
                if (crudp.Value.Substring(1, 1) == "0") requirement.Read = false;
                if (crudp.Value.Substring(2, 1) == "0") requirement.Update = false;
                if (crudp.Value.Substring(3, 1) == "0") requirement.Delete = false;
                if (crudp.Value.Substring(4, 1) == "0") requirement.Print = false;
            }

            return crudp;
        }
    }
}
