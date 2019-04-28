using System;
using FlatRent.Entities;

namespace FlatRent.Models.Dtos
{
    public class AgreementDetails
    {
        public Guid Id { get; set; }
        public AgreementStatus Status { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public string Comments { get; set; }
        public float Price { get; set; }

        public ShortFlatDetails Flat { get; set; }

        public ShortAgreementUserInfo Tenant { get; set; }
        public ShortAgreementUserInfo Owner { get; set; }
    }

    public class ShortAgreementUserInfo : ShortUserInfo
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}