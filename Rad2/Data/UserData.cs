using System.ComponentModel.DataAnnotations;

namespace Rad2.Data
{
    public class UserData
    {
        [EmailAddress]
        [Required]
        public string LoginName { get; set; } = string.Empty;

        [Required]
        public string UserRole { get; set; } = string.Empty;
    }
}
