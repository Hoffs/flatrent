using System;
using FlatRent.Entities;
using FlatRent.Models.Dtos;

namespace FlatRent.Dtos
{
    public class ShortAgreementDetails
    {
        public Guid Id { get; set; }
        public AgreementStatus Status { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public string FlatName { get; set; }
        public ShortAddress Address { get; set; }

        public ShortUserInfo Tenant { get; set; }
        public ShortUserInfo Owner { get; set; }
    }
}