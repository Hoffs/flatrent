using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FlatRent.Entities
{
    public class Photo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(64000)]
        [Required]
        public byte[] PhotoBytes { get; set; }

        [JsonIgnore]
        [Required]
        public virtual Flat Flat { get; set; }
    }
}