using System.ComponentModel.DataAnnotations;
using FlatRent.Constants;

namespace FlatRent.Models.Requests
{
    public class RegistrationForm
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

        public string Description { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        public bool Avatar { get; set; }
    }
}