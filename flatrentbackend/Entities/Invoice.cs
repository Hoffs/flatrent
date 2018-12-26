using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlatRent.Entities
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public float AmountToPay { get; set; }
        [Required]
        public float AmountPaid { get; set; }
        [Required]
        public DateTime PaidDate { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public virtual RentAgreement RentAgreement { get; set; }
    }
}