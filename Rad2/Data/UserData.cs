using System.ComponentModel.DataAnnotations;

namespace Rad2.Data
{
    public class UserData
    {
        [Required]
        public string UserRole { get; set; } = string.Empty;

        public bool Create {  get; set; } = true; 
        public bool Read { get; set; } = true;
        public bool Update { get; set; } = true;
        public bool Delete { get; set; } = true;
        public bool Print { get; set; } = true;
    }
}
