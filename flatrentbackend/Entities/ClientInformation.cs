using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FlatRent.Entities
{
    public class ClientInformation : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        [InverseProperty("ClientInformation")]
        public virtual List<RentAgreement> Agreements { get; set; }

        [JsonIgnore]
        [InverseProperty("ClientInformation")]
        public virtual List<Fault> Faults { get; set; }
        
        [JsonIgnore]
        [InverseProperty("ClientInformation")]
        public virtual User User { get; set; }
    }
}