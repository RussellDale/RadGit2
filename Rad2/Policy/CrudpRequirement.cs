using Microsoft.AspNetCore.Authorization;

namespace Rad2.Policy
{
    public class CrudpRequirement : IAuthorizationRequirement
    {
        public CrudpRequirement(bool create, bool read, bool update, bool delete, bool print, bool admin)
        {
            Create = create;
            Read = read;
            Update = update;
            Delete = delete;
            Print = print;
            Admin = admin;
        }
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Print { get; set; }
        public bool Admin { get; set; }
    }
}
