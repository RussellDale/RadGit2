using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Rad2.Policy
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            MinimumAgeRequirement requirement)
        {
            var dateOfBirthClaim = context.User.FindFirst(c => c.Type == "Age");

            if (dateOfBirthClaim is null)
                return Task.CompletedTask;

            var calculatedAge = int.Parse(dateOfBirthClaim.Value);

            if (calculatedAge >= requirement.MinimumAge)
                context.Succeed(requirement);
            else
                context.Fail();

            return Task.CompletedTask;
        }
    }
}
