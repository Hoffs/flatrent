using System.ComponentModel.DataAnnotations;

namespace FlatRent.Models
{
    public class RegistrationForm
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }
        [Required]
        [MaxLength(64)]
        [MinLength(8)]
        public string Password { get; set; }
        [Required]
        [MaxLength(50)]
        [Phone]
        public string PhoneNumber { get; set; }

        public string Description { get; set; }
    }
}