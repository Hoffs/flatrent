using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FlatRent.Entities
{
    public class Attachment : File
    {
        [Required, ForeignKey("Message")]
        public Guid MessageId { get; set; }
        [JsonIgnore]
        public virtual Message Message { get; set; }
    }
}