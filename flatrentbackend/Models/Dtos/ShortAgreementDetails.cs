using System;
using FlatRent.Entities;

namespace FlatRent.Models.Dtos
{
    public class ShortAgreementDetails
    {
        public Guid Id { get; set; }
        public AgreementStatus Status { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public string FlatName { get; set; }
        public ShortAddress Address { get; set; }
    }
}