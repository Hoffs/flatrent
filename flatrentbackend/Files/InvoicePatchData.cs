using System;

namespace FlatRent.Files
{
    public class InvoicePatchData
    {
        public Guid InvoiceNo { get; set; }
        public Guid AgreementNo { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string InvoiceFrom { get; set; }
        public string InvoiceTo { get; set; }
        public string InvoiceDue { get; set; }

        public string AdditionalRows { get; set; }

        public float Price { get; set; }
        public float TotalPrice { get; set; }
    }
}