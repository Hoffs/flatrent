using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FlatRent.Constants;
using FlatRent.Models.Attributes;

namespace FlatRent.Models.Requests
{
    public class FlatUpdateForm
    {
        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(64, ErrorMessage = Errors.MaxLength)]
        public string Name { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        [Range(1, int.MaxValue, ErrorMessage = Errors.Range)]
        public float Price { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        [MaxLength(30, ErrorMessage = Errors.MaxLengthFeatures)]
        [EnumerableStringMaxLength(128, ErrorMessage = Errors.MaxLengthFeatureSymbols)]
        public IEnumerable<string> Features { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        public string Description { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        public bool IsFurnished { get; set; }

        [Range(1, 3650)]
        [Required(ErrorMessage = Errors.Required)]
        public int MinimumRentDays { get; set; }

        [Required(ErrorMessage = Errors.Required)]
        public string TenantRequirements { get; set; }

        [MinLength(3, ErrorMessage = Errors.MinLengthFiles)]
        [MaxLength(32, ErrorMessage = Errors.MaxLengthFiles)]
        public IEnumerable<FileMetadata> Images { get; set; }
    }
}