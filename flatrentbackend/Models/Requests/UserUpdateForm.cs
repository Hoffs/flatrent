using System.ComponentModel.DataAnnotations;
using FlatRent.Constants;

namespace FlatRent.Models.Requests
{
    public class UserUpdateForm
    {
        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(50, ErrorMessage = Errors.MaxLength)]
        [Phone(ErrorMessage = Errors.Phone)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(34, ErrorMessage = Errors.MaxLength)]
        public string BankAccount { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(1000, ErrorMessage = Errors.MaxLength)]
        public string About { get; set; }
    }
}