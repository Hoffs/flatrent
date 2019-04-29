using System;
using System.Collections.Generic;

namespace FlatRent.Models.Responses
{
    public class CreatedFlatResponse
    {
        public Guid Id { get; set; }
        public Dictionary<Guid, string> Images { get; set; }

        public CreatedFlatResponse(Guid id, IEnumerable<KeyValuePair<Guid, string>> images)
        {
            Id = id;
            Images = new Dictionary<Guid, string>(images);
        }
    }
}