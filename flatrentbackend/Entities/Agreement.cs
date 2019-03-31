using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlatRent.Entities
{
    public class Agreement : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
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
        [ForeignKey("Renter")]
        public Guid RenterId { get; set; }
        public virtual User Renter { get; set; }

        [Required] 
        public Guid FlatId { get; set; }
        public virtual Flat Flat { get; set; }

        [InverseProperty("Agreement")]
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}