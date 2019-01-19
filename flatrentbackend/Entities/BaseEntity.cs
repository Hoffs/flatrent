using System;
using Newtonsoft.Json;

namespace FlatRent.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [JsonIgnore]
        public bool Deleted { get; set; }
    }
}