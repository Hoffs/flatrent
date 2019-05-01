using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FlatRent.Constants;

namespace FlatRent.Models.Requests
{
    public class MessageForm
    {
        [MaxLength(5000, ErrorMessage = Errors.MaxLength)]
        public string Content { get; set; }

        [MaxLength(8, ErrorMessage = Errors.MaxLengthFiles)]
        public IEnumerable<FileMetadata> Attachments { get; set; }
    }
}