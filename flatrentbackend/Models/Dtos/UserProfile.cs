using System;
using System.Collections.Generic;
using FlatRent.Entities;

namespace FlatRent.Models.Dtos
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string About { get; set; }
        public Guid AvatarId { get; set; }
//        public IEnumerable<ShortFlatDetails> Flats { get; set; }
        public int TenantAgreementCount { get; set; }
        public int OwnerAgreementCount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}