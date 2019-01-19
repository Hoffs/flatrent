using System.ComponentModel.DataAnnotations;
using FlatRent.Constants;

namespace FlatRent.Models.Requests
{
    public class LoginForm
    {
        [Required(ErrorMessage = Errors.Required)]
        [EmailAddress(ErrorMessage = Errors.EmailAddress)]
        [MaxLength(256)]
        public string Email { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(64, ErrorMessage = Errors.MaxLength)]
        [MinLength(8, ErrorMessage = Errors.MinLength)]
        public string Password { get; set; }
    }
}