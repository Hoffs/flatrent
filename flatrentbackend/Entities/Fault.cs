using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlatRent.Entities
{
    public class Fault : AuthoredBaseEntity
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public bool Repaired { get; set; }
        [Required]
        public float Price { get; set; }

        // Associated Agreement
        [Required]
        [ForeignKey("Agreement")]
        public Guid AgreementId { get; set; }
        public virtual Agreement Agreement { get; set; }

        // Associated Flat
        [Required]
        [ForeignKey("Flat")]
        public Guid FlatId { get; set; }
        public virtual Flat Flat { get; set; }

        [InverseProperty("AssociatedFault")]
        public virtual ICollection<Conversation> Conversations { get; set; }
    }
}