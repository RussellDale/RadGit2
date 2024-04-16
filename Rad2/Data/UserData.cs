using System.ComponentModel.DataAnnotations;

namespace Rad2.Data
{
    public class UserData
    {
        [Required]
        public string UserRole { get; set; } = string.Empty;
    }
}
