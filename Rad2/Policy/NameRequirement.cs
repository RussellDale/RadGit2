using Microsoft.AspNetCore.Authorization;

namespace Rad2.Policy
{
    public class NameRequirement : IAuthorizationRequirement
    {
        public NameRequirement(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
