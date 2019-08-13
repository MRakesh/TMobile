
using System.ComponentModel.DataAnnotations;
namespace App2.Models
{
    public class LoginModel
    {
        [Required, MaxLength(50), EmailAddress]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
