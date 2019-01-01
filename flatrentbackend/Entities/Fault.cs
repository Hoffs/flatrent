using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlatRent.Entities
{
    public class Fault : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public bool Repaired { get; set; }
        [Required]
        public float Price { get; set; }

        [Required]
        public virtual Flat Flat { get; set; }

        [Required]
        public virtual ClientInformation ClientInformation { get; set; }

        [Required]
        public virtual EmployeeInformation EmployeeInformation { get; set; }
    }
}