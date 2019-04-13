using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FlatRent.Entities
{
    public class Address : AuthoredBaseEntity
    {
        [Required]
        [MaxLength(128)]
        public string Street { get; set; }
        [Required]
        public string HouseNumber { get; set; }
        [Required]
        public string FlatNumber { get; set; }
        [Required]
        [MaxLength(128)]
        public string City { get; set; }
        [Required]
        [MaxLength(128)]
        public string Country { get; set; }
        [Required]
        [MaxLength(24)]
        public string PostCode { get; set; }

        [JsonIgnore]
        [InverseProperty("Address")] 
        public virtual Flat Flat { get; set; }

        public override string ToString()
        {
            return $"{Street} {HouseNumber}-{FlatNumber}, {City}, {Country} {PostCode}";
        }
    }
}