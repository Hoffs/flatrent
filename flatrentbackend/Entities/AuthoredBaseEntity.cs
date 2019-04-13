using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FlatRent.Entities
{
    public abstract class AuthoredBaseEntity : BaseEntity
    {
        [Required]
        [ForeignKey("Author")]
        [JsonIgnore]
        public Guid AuthorId { get; set; }
        [JsonIgnore]
        public virtual User Author { get; set; }
    }
}