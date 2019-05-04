using System;

namespace FlatRent.Models.Dtos
{
    public class ShortFaultDetails
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Repaired { get; set; }
        public float Price { get; set; }
    }
}