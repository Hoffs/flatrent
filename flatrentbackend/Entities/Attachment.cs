using System;
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

        [ForeignKey("Incident")]
        public Guid? IncidentId { get; set; }
        [JsonIgnore]
        public virtual Incident Incident { get; set; }
    }
}