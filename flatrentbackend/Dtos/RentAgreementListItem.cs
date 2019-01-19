using System;
using FlatRent.Entities;

namespace FlatRent.Dtos
{
    public class RentAgreementListItem
    {
        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string FlatName { get; set; }
        public Address FlatAddress { get; set; }

    }
}