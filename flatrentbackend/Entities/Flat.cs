using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FlatRent.Entities
{
    public class Flat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(64)]
        [Required]
        public string Name { get; set; }
        [Required]
        public float Area { get; set; }
        [Required]
        public int Floor { get; set; }
        [Required]
        public int RoomCount { get; set; }
        [Required]
        public float Price { get; set; }
        [Range(0, 999999)]
        [Required]
        public int YearOfConstruction { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public virtual Owner Owner { get; set; }

        [JsonIgnore]
        public Guid AddressId { get; set; }
        
        [Required]
        public virtual Address Address { get; set; }

        [JsonIgnore]
        [InverseProperty("Flat")]
        public virtual List<RentAgreement> Agreements { get; set; }

        [InverseProperty("Flat")]
        public virtual List<Photo> Photos { get; set; }

        [JsonIgnore]
        [InverseProperty("Flat")]
        public virtual List<Fault> Faults { get; set; }
    }
}