using System.ComponentModel.DataAnnotations;

namespace FlatRent.Models
{
    public class LoginForm
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }
        [Required]
        [MaxLength(64)]
        [MinLength(8)]
        public string Password { get; set; }
    }
}