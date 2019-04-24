﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FlatRent.Entities
{
    public class Agreement : AuthoredBaseEntity
    {
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }

        [MaxLength(65536)]
        public string Comments { get; set; }

        [Required]
        [ForeignKey("Status")]
        public int StatusId { get; set; }
        [Required]
        public virtual AgreementStatus Status { get; set; }

        [Required]
        [ForeignKey("Tenant")]
        public Guid TenantId { get; set; }
        public virtual User Tenant { get; set; }

        [Required] 
        public Guid FlatId { get; set; }
        public virtual Flat Flat { get; set; }

        [JsonIgnore]
        [InverseProperty("Agreement")]
        public virtual ICollection<Invoice> Attachments { get; set; }

        [JsonIgnore]
        [InverseProperty("Agreement")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [JsonIgnore]
        [InverseProperty("Agreement")]
        public virtual ICollection<Fault> Faults { get; set; }

        [JsonIgnore]
        [InverseProperty("AssociatedAgreement")]
        public virtual ICollection<Conversation> Conversations { get; set; }
    }
}