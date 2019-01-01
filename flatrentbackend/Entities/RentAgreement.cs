using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlatRent.Entities
{
    public class RentAgreement : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public bool Verified { get; set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
        [Required]
        public string Comments { get; set; }

        [Required]
        public virtual ClientInformation ClientInformation { get; set; }

        [Required]
        public virtual Flat Flat { get; set; }

        [InverseProperty("RentAgreement")]
        public virtual List<Invoice> Invoices { get; set; }
    }
}