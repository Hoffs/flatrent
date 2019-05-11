using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FlatRent.Constants;

namespace FlatRent.Models.Requests
{
    public class IncidentForm
    {
        [MaxLength(128, ErrorMessage = Errors.MaxLength)]
        public string Name { get; set; }

        [MaxLength(2000, ErrorMessage = Errors.MaxLength)]
        public string Description { get; set; }

        [MaxLength(8, ErrorMessage = Errors.MaxLength)]
        public IEnumerable<FileMetadata> Attachments { get; set; }
    }
}