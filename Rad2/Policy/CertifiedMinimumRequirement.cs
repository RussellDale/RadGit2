using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Rad2.Policy
{
    public class CertifiedMinimumRequirement : IAuthorizationRequirement
    {
        public CertifiedMinimumRequirement(bool certified, int certifiedNumberOfYears) 
        {
            Certified = certified;
            CertifiedNumberOfYears = certifiedNumberOfYears;
        }
        public bool Certified { get; }
        public int CertifiedNumberOfYears { get; }
    }
}
