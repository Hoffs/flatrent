
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FlatRent.Entities
{
    public class Avatar : BaseEntity
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(512)]
        [JsonIgnore]
        public string MimeType { get; set; }

        [Required]
        [MaxLength(65536)]
        [JsonIgnore]
        public byte[] Bytes { get; set; }

        [JsonIgnore]
        [NotMapped]
        public bool IsUploaded => (Bytes?.Length ?? 0) > 0;
    }
}