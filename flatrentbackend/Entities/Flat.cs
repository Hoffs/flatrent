﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
        public int RoomCount { get; set; }
        [Required]
        public float Price { get; set; }
        [Range(0, 999999)]
        [Required]
        public int YearOfConstruction { get; set; }
        [Required]
        public string Description { get; set; }

        // Is ready to be shown/rented to public
        [Required]
        public bool IsPublished { get; set; }

        // Should be shown in listings
        [Required]
        public bool IsPublic { get; set; }

        [NotMapped]
        public bool IsRented =>
            ActiveAgreement != null;

        [NotMapped]
        public bool IsAvailableForRent =>
            !IsRented && IsPublished;

        [NotMapped]
        public Agreement ActiveAgreement =>
            Agreements.FirstOrDefault(x => x.From.Date >= DateTime.Now.Date && x.To <= DateTime.Now.Date && !x.Deleted);

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

        [JsonIgnore]
        [InverseProperty("Flat")]
        public virtual ICollection<Fault> Faults { get; set; }

        [JsonIgnore]
        [InverseProperty("AssociatedFlat")]
        public virtual ICollection<Conversation> Conversations { get; set; }
    }
}