using System;
using System.Security.Claims;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public Task HandleRequirementAsync3(IList<Claim> claims, CrudpRequirement requirement)
        {
            Claim? crudp = null;

            crudp = Handle2(claims, requirement, crudp);

            return Task.CompletedTask;
        }
        private Claim? Handle(ClaimsPrincipal user, CrudpRequirement requirement, Claim? crudp)
        {
            crudp = user.FindFirst(c => c.Type == "CRUDP");

            return Claim(crudp, requirement);
        }
        private Claim? Handle2(IList<Claim> claims, CrudpRequirement requirement, Claim? crudp)
        {
            crudp = claims.First(c => c.Type == "CRUDP");

            return Claim(crudp, requirement);
        }

        private Claim? Claim(Claim? crudp, CrudpRequirement requirement)
        {
            if (crudp is not null)
            {
                if (crudp.Value.Substring(0, 1) == "0") requirement.Create = false;
                if (crudp.Value.Substring(1, 1) == "0") requirement.Read   = false;
                if (crudp.Value.Substring(2, 1) == "0") requirement.Update = false;
                if (crudp.Value.Substring(3, 1) == "0") requirement.Delete = false;
                if (crudp.Value.Substring(4, 1) == "0") requirement.Print  = false;
                if (crudp.Value.Substring(5, 1) == "0") requirement.Admin  = false;
            }

            return crudp;
        }
    }
}
