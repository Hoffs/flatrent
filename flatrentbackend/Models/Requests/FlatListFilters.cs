namespace FlatRent.Models.Requests
{
    public class FlatListFilters
    {
        public int? FloorFrom { get; set; }
        public int? FloorTo { get; set; }

        public float? PriceFrom { get; set; }
        public float? PriceTo { get; set; }

        public float? AreaFrom { get; set; }

        public int? RoomCountFrom { get; set; }
        public int? RentDays { get; set; }

        public string City { get; set; }
    }
}