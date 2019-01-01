using System.ComponentModel.DataAnnotations;
using FlatRent.Constants;
using FlatRent.Validators;

namespace FlatRent.Models
{
    public class RegistrationEmployeeForm
    {
        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(50, ErrorMessage = Errors.MaxLength)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(50, ErrorMessage = Errors.MaxLength)]
        public string LastName { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [EmailAddress(ErrorMessage = Errors.EmailAddress)]
        [MaxLength(256, ErrorMessage = Errors.MaxLength)]
        public string Email { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(64, ErrorMessage = Errors.MaxLength)]
        [MinLength(8, ErrorMessage = Errors.MinLength)]
        public string Password { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(50, ErrorMessage = Errors.MaxLength)]
        [Phone(ErrorMessage = Errors.Phone)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(64, ErrorMessage = Errors.MaxLength)]
        public string Position { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(64, ErrorMessage = Errors.MaxLength)]
        [ValidDepartment(ErrorMessage = Errors.ValidDepartment)]
        public string Department { get; set; }
    }
}