using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FlatRent.Entities
{
    public class Fault : AuthoredBaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool Repaired { get; set; }

        public float Price { get; set; }

        // Associated Agreement
        [Required]
        [ForeignKey("Agreement")]
        public Guid AgreementId { get; set; }
        public virtual Agreement Agreement { get; set; }

        [JsonIgnore]
        [InverseProperty("Fault")]
        public virtual ICollection<Attachment> Attachments { get; set; }

        [NotMapped]
        public static Func<Fault, bool> NotInvoicedFaultsFunc =>
            (fault) => fault.Repaired && fault.InvoiceId == null;

        [ForeignKey("Conversation")]
        public Guid ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }

        // Assigned invoice
        [ForeignKey("Invoice")]
        public Guid? InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}