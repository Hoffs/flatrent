using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FlatRent.Constants;
using FlatRent.Models.Attributes;
using Microsoft.AspNetCore.Http;

namespace FlatRent.Models.Requests.Flat
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
        public int Floor { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [Range(-8, 128, ErrorMessage = Errors.Range)]
        public int TotalFloors { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        [Range(1, 128, ErrorMessage = Errors.Range)]
        public int RoomCount { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        [Range(1, int.MaxValue, ErrorMessage = Errors.Range)]
        public float Price { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        [Range(1, 2050, ErrorMessage = Errors.Range)]
        public int YearOfConstruction { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(30, ErrorMessage = Errors.MaxLengthFeatures)]
        [EnumerableStringMaxLength(128, ErrorMessage = Errors.MaxLengthFeatureSymbols)]
        public IEnumerable<string> Features { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        public string Description { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        public bool IsFurnished { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        public bool IsPublished { get; set; }

        [Range(1, 3650)]
        [Required(ErrorMessage = Errors.Required)]
        public int MinimumRentDays { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        public string TenantRequirements { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(128, ErrorMessage = Errors.MaxLength)]
        public string Street { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(8, ErrorMessage = Errors.MaxLength)]
        public string HouseNumber { get; set; }
        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(8, ErrorMessage = Errors.MaxLength)]
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

        [MinLength(3, ErrorMessage = Errors.MinLengthImages)]
        [MaxLength(32, ErrorMessage = Errors.MaxLengthImages)]
        public IEnumerable<FileMetadata> Images { get; set; }
    }
}