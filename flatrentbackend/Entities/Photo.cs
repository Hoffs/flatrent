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

        [Required]
        [MaxLength(65536)]
        public byte[] PhotoBytes { get; set; }

        [Required]
        [JsonIgnore]
        public virtual Flat Flat { get; set; }
    }
}