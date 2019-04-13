using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlatRent.Entities
{
    public class Conversation : AuthoredBaseEntity
    {
        public string Subject { get; set; }

        [ForeignKey("Recipient")]
        public Guid RecipientId { get; set; }
        public virtual User Recipient { get; set; }

        [ForeignKey("AssociatedFlat")]
        public Guid FlatId { get; set; }
        public virtual Flat AssociatedFlat { get; set; }

        [ForeignKey("AssociatedAgreement")]
        public Guid AgreementId { get; set; }
        public virtual Agreement AssociatedAgreement { get; set; }

        [ForeignKey("AssociatedFault")]
        public Guid FaultId { get; set; }
        public virtual Fault AssociatedFault { get; set; }

        [InverseProperty("Conversation")]
        public virtual ICollection<Message> Messages { get; set; }
    }
}