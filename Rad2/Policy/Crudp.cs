using Microsoft.AspNetCore.Authorization;

namespace Rad2.Policy
{
    public class Crudp : IAuthorizationRequirement
    {
        public Crudp(bool create, bool read, bool update, bool delete, bool print)
        {
            Create = create;
            Read = read;
            Update = update;
            Delete = delete;
            Print = print;
        }
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Print { get; set; }
    }
}
