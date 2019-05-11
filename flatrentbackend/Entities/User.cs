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

        [MaxLength(1000)]
        public string About { get; set; }

        // TODO: Add payment information/bank account etc.
        public string BankAccount { get; set; }

        [ForeignKey("Avatar")]
        public Guid AvatarId { get; set; } = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public virtual Avatar Avatar { get; set; }

        [JsonIgnore]
        [Required]
        public int TypeId { get; set; }
        [JsonIgnore]
        public virtual UserType Type { get; set; }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        [JsonIgnore]
        [InverseProperty("Tenant")]
        public virtual ICollection<Agreement> TenantAgreements { get; set; }

        [JsonIgnore]
        [InverseProperty("Author")]
        public virtual ICollection<Flat> Flats { get; set; }

        [NotMapped]
        public IEnumerable<Agreement> OwnerAgreements => Flats.SelectMany(x => x.Agreements).Where(a => !a.Deleted);

        [JsonIgnore]
        [InverseProperty("Author")]
        public virtual ICollection<Conversation> StartedConversations { get; set; }

        [JsonIgnore]
        [InverseProperty("Recipient")]
        public virtual ICollection<Conversation> RecipientConversations { get; set; }

        [JsonIgnore]
        [InverseProperty("Author")]
        public virtual ICollection<Incident> RegisteredIncidents { get; set; }
    }
}