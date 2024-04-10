using Microsoft.AspNetCore.Authorization;

namespace Rad2.Policy
{
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public MinimumAgeRequirement(int minimumAge) 
        {
            MinimumAge = minimumAge;
        }
        public int MinimumAge { get; }
    }
}
