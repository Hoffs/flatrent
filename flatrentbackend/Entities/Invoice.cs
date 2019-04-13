using System;
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

        [Required]
        public float AmountPaid { get; set; }
        [Required]
        public DateTime PaidDate { get; set; }

        [NotMapped]
        public bool IsPaid => AmountPaid >= AmountToPay;
        [NotMapped]
        public bool IsOverdue => PaidDate == default(DateTime) && DateTime.UtcNow.Date > DueDate.Date;

        [Required]
        [ForeignKey("Agreement")]
        public Guid AgreementId { get; set; }
        public virtual Agreement Agreement { get; set; }
    }
}