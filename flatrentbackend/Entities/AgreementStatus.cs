using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace FlatRent.Entities
{
    public class AgreementStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] public string Name { get; set; }

        [JsonIgnore]
        [InverseProperty("Status")]
        public virtual List<Agreement> Agreements { get; set; }

        [NotMapped]
        public static IEnumerable<AgreementStatus> ExistingAgreementStatuses =>
            typeof(Statuses).GetFields().Select(field => new AgreementStatus{ Id = (int) field.GetRawConstantValue(), Name = field.Name });

        [NotMapped]
        public static class Statuses
        {
            public const int Requested = 1;
            public const int Accepted = 2;
            public const int Rejected = 3;
            public const int Expired = 4;
            public const int Ended = 5;
        }
    }
}