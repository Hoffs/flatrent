using System;
using System.Collections.Generic;

namespace FlatRent.Models.Dtos
{
    public class InvoiceDetails
    {
        public float AmountToPay { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
        public bool IsOverdue { get; set; }
        public IEnumerable<ShortFaultDetails> Faults { get; set; }
    }
}