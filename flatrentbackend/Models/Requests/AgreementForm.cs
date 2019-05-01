using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FlatRent.Constants;
using FlatRent.Models.Attributes;

namespace FlatRent.Models.Requests
{
    public class AgreementForm
    {
        [Required(ErrorMessage = Errors.Required)]
        [DateAfter(DaysAfter = 7, ErrorMessage = Errors.DateAfter)]
        [DateBefore(DaysToAdd = 30, ErrorMessage = Errors.DateBefore)]
        public DateTime From { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        [DateAfter(DaysAfter = 7, ErrorMessage = Errors.DateAfter)]
        public DateTime To { get; set; }

        [MaxLength(5000, ErrorMessage = Errors.MaxLength)]
        public string Comments { get; set; }

        [MaxLength(8, ErrorMessage = Errors.MaxLength)]
        public IEnumerable<FileMetadata> Attachments { get; set; }
    }
}