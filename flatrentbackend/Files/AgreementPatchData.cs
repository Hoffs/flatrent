using System;

namespace FlatRent.Files
{
    public class AgreementPatchData
    {
        public Guid AgreementNo { get; set; }
        public string AgreementFrom { get; set; }
        public string AgreementTo { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Owner { get; set; }
        public string Client { get; set; }
        public string Address { get; set; }
        public float Area { get; set; }
        public float Price { get; set; }
    }
}