using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FlatRent.Entities
{
    public class Attachment : File
    {
        [ForeignKey("Message")]
        public Guid? MessageId { get; set; }
        [JsonIgnore]
        public virtual Message Message { get; set; }

        [ForeignKey("Agreement")]
        public Guid? AgreementId { get; set; }
        [JsonIgnore]
        public virtual Agreement Agreement { get; set; }

        [ForeignKey("Fault")]
        public Guid? FaultId { get; set; }
        [JsonIgnore]
        public virtual Fault Fault { get; set; }
    }
}