using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlatRent.Entities
{
    public class ClientInformation : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Description { get; set; }

        [InverseProperty("ClientInformation")]
        public virtual List<RentAgreement> Agreements { get; set; }

        [InverseProperty("ClientInformation")]
        public virtual List<Fault> Faults { get; set; }
    }
}