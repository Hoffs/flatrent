using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace FlatRent.Entities
{
    public class Flat : AuthoredBaseEntity
    {
        [MaxLength(64)]
        [Required]
        public string Name { get; set; }
        [Required]
        public float Area { get; set; }
        [Required]
        public int Floor { get; set; }
        [Required]
        public int TotalFloors { get; set; }
        [Required]
        public int RoomCount { get; set; }
        [Required]
        public float Price { get; set; }

        [Range(0, 2100)]
        [Required]
        public int YearOfConstruction { get; set; }

        public IEnumerable<string> Features { get; set; }

        [Required]
        [MaxLength(10240)]
        public string Description { get; set; }

        [Required]
        public bool IsFurnished { get; set; }

        [Range(1, 3650)]
        [Required]
        public int MinimumRentDays { get; set; }

        [Required]
        [MaxLength(5120)]
        public string TenantRequirements { get; set; }

        // Is ready to be shown/rented to public
        [Required]
        public bool IsPublished { get; set; }

//        [NotMapped]
//        public bool IsRented =>
//            ActiveAgreement != null;

//        [NotMapped]
//        public bool IsAvailableForRent =>
//            !IsRented && IsPublished;

        [NotMapped]
        public Agreement ActiveAgreement =>
            Agreements.FirstOrDefault(y =>
                !y.Deleted && y.StatusId == AgreementStatus.Statuses.Accepted
            );

        [NotMapped]
        public static Expression<Func<Flat, bool>> HasNoActiveAgreement =>
            //            (flat) => flat.Agreements.FirstOrDefault(y =>
            //                !y.Deleted && y.StatusId == AgreementStatus.Statuses.Accepted
            //                           && y.From.Date >= DateTime.Now.Date && y.To <= DateTime.Now.Date
            //            ) == null;
            (flat) => flat.Agreements.FirstOrDefault(y =>
                !y.Deleted && y.StatusId == AgreementStatus.Statuses.Accepted
            ) == null;
//            Agreements.FirstOrDefault(y =>
//                !y.Deleted && y.StatusId == AgreementStatus.Statuses.Accepted
//                           && y.From.Date >= DateTime.Now.Date && y.To <= DateTime.Now.Date
//            );

        [NotMapped]
        public Image CoverImage =>
            Images.FirstOrDefault();

        // required where?
        [JsonIgnore]
        [ForeignKey("Address")]
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }

        [JsonIgnore]
        [InverseProperty("Flat")]
        public virtual ICollection<Agreement> Agreements { get; set; }

        [InverseProperty("Flat")]
        public virtual ICollection<Image> Images { get; set; }
    }
}