using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Rad2.Policy
{
    public class CertifiedMinimumHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context) 
        { 
            var pendingRequirements = context.PendingRequirements.ToList();

            foreach(var requirements in pendingRequirements)
            {
                if(requirements is CertifiedMinimumRequirement)
                {
                    var certifiedMinimumRequirement = requirements as CertifiedMinimumRequirement;
                    var certified = context.User.FindFirst(c => c.Type == "Certified");
                    var certifiedNumberOfYears = context.User.FindFirst(c => c.Type == "CertifiedNumberOfYears");

                    bool isCertified = false;
                    Boolean.TryParse(certified?.Value, out isCertified);

                    int yearsCertified = 0;
                    int.TryParse(certifiedNumberOfYears?.Value, out yearsCertified);

                    if (isCertified && yearsCertified >= 5) 
                        context.Succeed(requirements);
                }
                else if(requirements is MinimumAgeRequirement)
                { 
                    var minimumAgeRequirement = requirements as MinimumAgeRequirement;
                    var dateOfBirthClaim = context.User.FindFirst(c => c.Type == "Age");

                    if (dateOfBirthClaim is null)
                        context.Fail();

                    var calculatedAge = int.Parse(dateOfBirthClaim?.Value ?? "0");
                    calculatedAge *= 7;

                    if (calculatedAge >= minimumAgeRequirement?.MinimumAge)
                        context.Succeed(requirements);
                }
            }
            return Task.CompletedTask;
        }
    }
}
