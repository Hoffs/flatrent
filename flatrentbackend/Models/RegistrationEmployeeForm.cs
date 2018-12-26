using System.ComponentModel.DataAnnotations;
using FlatRent.Validators;

namespace FlatRent.Models
{
    public class RegistrationEmployeeForm
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

        [Required]
        [MaxLength(64)]
        public string Position { get; set; }
        [Required]
        [MaxLength(64)]
        [ValidDepartment]
        public string Department { get; set; }
    }
}