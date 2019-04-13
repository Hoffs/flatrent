using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FlatRent.Entities
{
    public class Image : File
    {
        [JsonIgnore]
        [Required, ForeignKey("Flat")]
        public Guid FlatId { get; set; }
        [JsonIgnore]
        public virtual Flat Flat { get; set; }
    }
}