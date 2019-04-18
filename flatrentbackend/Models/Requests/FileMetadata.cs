using System.ComponentModel.DataAnnotations;
using FlatRent.Constants;

namespace FlatRent.Models.Requests
{
    public class FileMetadata
    {
        [Required]
        [MaxLength(128, ErrorMessage = Errors.MaxLength)]
        public string Name { get; set; }
    }
}