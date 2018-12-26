using System;
using FlatRent.Entities;

namespace FlatRent.Dtos
{
    public class FlatListItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Area { get; set; }
        public int Floor { get; set; }
        public int RoomCount { get; set; }
        public float Price { get; set; }
        public int YearOfConstruction { get; set; }
        public FlatListItemAddress Address { get; set; }
    }

    public class FlatListItemAddress
    {
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string FlatNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
    }
}