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
        public bool IsName ()
        {
            string name = Name.Trim().ToLower();
            
            bool isName = false;
            
            if(name.Length == 0)
               isName = false;
            else 
            {
                if(name.Length == 1 && name.Substring(0, 1) == "*")
                    isName = false;
                else
                {
                    isName = true;
                }
            }
            
            return isName;
        }
    }
}
