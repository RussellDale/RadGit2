using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Rad2.Policy
{
    public class CrudpHandler : AuthorizationHandler<Crudp>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Crudp requirement)
        {
            var crudp = context.User.FindFirst(c => c.Type == "CRUDP");

            if (crudp is null)
                return Task.CompletedTask;

            if (crudp.Value.Substring(0, 1) == "0") requirement.Create = false;
            if (crudp.Value.Substring(1, 1) == "0") requirement.Read = false;
            if (crudp.Value.Substring(2, 1) == "0") requirement.Update = false;
            if (crudp.Value.Substring(3, 1) == "0") requirement.Delete = false;
            if (crudp.Value.Substring(4, 1) == "0") requirement.Print = false;

            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
