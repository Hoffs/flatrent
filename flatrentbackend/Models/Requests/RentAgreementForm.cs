using System;
using System.ComponentModel.DataAnnotations;
using FlatRent.Attributes;
using FlatRent.Constants;

namespace FlatRent.Models.Requests
{
    public class RentAgreementForm
    {
        public Guid ClientId { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        [DateAfter(DaysAfter = 7, ErrorMessage = Errors.DateAfter)]
        public DateTime From { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        [DateAfter(DaysAfter = 7 + 30, ErrorMessage = Errors.DateAfter)]
        public DateTime To { get; set; }

        public string Comments { get; set; }
    }
}