using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlatRent.Entities
{
    public class EmployeeInformation : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(64)]
        [Required]
        public string Position { get; set; }
        [MaxLength(64)]
        [Required]
        public string Department { get; set; }

        [InverseProperty("EmployeeInformation")]
        public virtual User User { get; set; }

        [InverseProperty("EmployeeInformation")]
        public virtual List<Fault> Faults { get; set; }
    }
}