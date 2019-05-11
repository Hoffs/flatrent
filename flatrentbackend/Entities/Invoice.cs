using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlatRent.Entities
{
    public class Invoice : BaseEntity
    {
        [Required]
        public float AmountToPay { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? PaidDate { get; set; }

        [Required]
        public bool IsValid { get; set; }

        [Required]
        public bool IsPaid { get; set; }

        [Required]
        public DateTime InvoicedPeriodFrom { get; set; }
        [Required]
        public DateTime InvoicedPeriodTo { get; set; }

        [NotMapped]
        public bool IsOverdue => PaidDate == null && DateTime.UtcNow.Date > DueDate.Date;

        [Required]
        [ForeignKey("Agreement")]
        public Guid AgreementId { get; set; }
        public virtual Agreement Agreement { get; set; }

        [InverseProperty("Invoice")]
        public virtual ICollection<Incident> Incidents { get; set; }
    }
}