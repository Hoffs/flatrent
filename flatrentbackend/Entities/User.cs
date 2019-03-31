using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

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
        [JsonIgnore]
        [MaxLength(64)]
        [Required]
        public string Password { get; set; }

        [MaxLength(50)]
        [Required]
        public string PhoneNumber { get; set; }

        [MaxLength(64000)]
        public string About { get; set; }


        // TODO: Add payment information/bank account etc.

        [JsonIgnore]
        [Required]
        public virtual int TypeId { get; set; }
        [JsonIgnore]
        public virtual UserType Type { get; set; }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        [JsonIgnore]
        [InverseProperty("Renter")]
        public virtual ICollection<Agreement> RenterAgreements { get; set; }

        [JsonIgnore]
        [InverseProperty("Owner")]
        public virtual ICollection<Flat> Flats { get; set; }

        [NotMapped]
        public IEnumerable<Agreement> OwnerAgreements => Flats.SelectMany(x => x.Agreements);
    }
}