using System;

namespace FlatRent.Models.Dtos
{
    public class ShortFlatDetails
    {
        public Guid Id { get; set; }
        public Guid ImageId { get; set; }
        public string Name { get; set; }
        public float Area { get; set; }
        public int Floor { get; set; }
        public int RoomCount { get; set; }
        public float Price { get; set; }
        public bool IsPublished { get; set; }
        public ShortAddress Address { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}