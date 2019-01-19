using System;
using System.ComponentModel.DataAnnotations;
using FlatRent.Constants;

namespace FlatRent.Models.Requests
{
    public class FlatForm
    {
        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(64, ErrorMessage = Errors.MaxLength)]
        public string Name { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [Range(1, 512, ErrorMessage = Errors.Range)]
        public float Area { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [Range(-8, 128, ErrorMessage = Errors.Range)]
        public int? Floor { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [Range(1, 128, ErrorMessage = Errors.Range)]
        public int RoomCount { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [Range(1, int.MaxValue, ErrorMessage = Errors.Range)]
        public float Price { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [Range(1, 3000, ErrorMessage = Errors.Range)]
        public int YearOfConstruction { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        public string Description { get; set; }


        public Guid? OwnerId { get; set; } 
        [Required(ErrorMessage = Errors.Required)]
        public string OwnerName { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = Errors.Alphanumeric)]
        [MinLength(4, ErrorMessage = Errors.MinLength)]
        public string Account { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [EmailAddress(ErrorMessage = Errors.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [Phone(ErrorMessage = Errors.Phone)]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(128, ErrorMessage = Errors.MaxLength)]
        public string Street { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        public string HouseNumber { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        public string FlatNumber { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(128, ErrorMessage = Errors.MaxLength)]
        public string City { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(128, ErrorMessage = Errors.MaxLength)]
        public string Country { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(24, ErrorMessage = Errors.MaxLength)]
        public string PostCode { get; set; }
    }
}