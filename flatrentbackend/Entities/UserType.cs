using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FlatRent.Entities
{
    public class UserType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        [InverseProperty("Type")]
        public virtual ICollection<User> Users { get; set; }

        [NotMapped]
        public static IEnumerable<UserType> ExistingTypes =>
            typeof(Types).GetFields().Select(field => new UserType { Id = (int)field.GetRawConstantValue(), Name = field.Name });

        [NotMapped]
        public static class Types
        {
            public const int Administrator = 1;
            public const int User = 2;
        }
    }
}