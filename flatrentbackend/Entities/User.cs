using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlatRent.Entities
{
    public class User : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }
        [MaxLength(256)]
        [Required]
        public string Email { get; set; }
        [MaxLength(64)]
        [Required]
        public string Password { get; set; }
        [MaxLength(50)]
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public virtual Guid TypeId { get; set; }
        public virtual UserType Type { get; set; }

        public Guid? EmployeeInformationId { get; set; }
        public virtual EmployeeInformation EmployeeInformation { get; set; }

        public Guid? ClientInformationId { get; set; }
        public virtual ClientInformation ClientInformation { get; set; }
    }
}