using System;
using System.Collections.Generic;

namespace FlatRent.Models.Dtos
{
    public class InvoiceDetails
    {
        public Guid Id { get; set; }
        public float AmountToPay { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public DateTime InvoicedPeriodFrom { get; set; }
        public DateTime InvoicedPeriodTo { get; set; }
        public bool IsPaid { get; set; }
        public bool IsValid { get; set; }
        public bool IsOverdue { get; set; }
        public IEnumerable<ShortIncidentDetails> Incidents { get; set; }
    }
}