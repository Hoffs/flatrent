using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FlatRent.Attributes;
using FlatRent.Constants;

namespace FlatRent.Models.Requests
{
    public class RentAgreementForm
    {
        [Required(ErrorMessage = Errors.Required)]
        [DateAfter(DaysAfter = 7, ErrorMessage = Errors.DateAfter)]
        public DateTime From { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        [DateAfter(DaysAfter = 7, ErrorMessage = Errors.DateAfter)]
        public DateTime To { get; set; }

        [MaxLength(64000, ErrorMessage = Errors.MaxLength)]
        public string Comments { get; set; }

        [MaxLength(8, ErrorMessage = Errors.MaxLength)]
        public IEnumerable<FileMetadata> Attachments { get; set; }
    }
}