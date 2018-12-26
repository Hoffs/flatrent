using System;
using System.ComponentModel.DataAnnotations;

namespace FlatRent.Models
{
    public class FlatForm
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        [Required]
        public float Area { get; set; }
        [Required]
        public int Floor { get; set; }
        [Required]
        public int RoomCount { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        [Range(0, 999999)]
        public int YearOfConstruction { get; set; }
        [Required]
        public string Description { get; set; }


        public Guid? OwnerId { get; set; } 
        [Required]
        public string OwnerName { get; set; }
        [Required]
        public string Account { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }


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
    }
}