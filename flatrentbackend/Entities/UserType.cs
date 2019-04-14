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

        [NotMapped] public string Role => Id.ToString();

        [NotMapped]
        public static IEnumerable<UserType> ExistingTypes => new [] { User, Administrator };

        [NotMapped]
        public static readonly UserType Administrator = new UserType
            { Id = 1, Name = nameof(Administrator) };

        [NotMapped]
        public static readonly UserType User = new UserType
            { Id = 2, Name = nameof(User) };

        public override bool Equals(object obj)
        {
            if (obj is UserType userType)
            {
                return Id == userType.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}