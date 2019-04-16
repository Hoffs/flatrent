using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FlatRent.Entities;

namespace FlatRent.Models.Dtos
{
    public class FlatDetails
    {
        public string Name { get; set; }
        public float Area { get; set; }
        public int Floor { get; set; }
        public int RoomCount { get; set; }
        public float Price { get; set; }
        public int YearOfConstruction { get; set; }
        public string Description { get; set; }

        public bool IsPublished { get; set; }
        public bool IsPublic { get; set; }
        public bool IsRented { get; set; }
        public bool IsAvailableForRent { get; set; }
        public Address Address { get; set; }
        public ShortUserInfo Owner { get; set; }
        public IEnumerable<Image> Images { get; set; }
    }
}