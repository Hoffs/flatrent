using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FlatRent.Constants;

namespace FlatRent.Models.Requests
{
    public class FaultForm
    {
        [MaxLength(5000, ErrorMessage = Errors.MaxLength)]
        public string Comments { get; set; }

        [MaxLength(8, ErrorMessage = Errors.MaxLength)]
        public IEnumerable<FileMetadata> Attachments { get; set; }
    }
}