using System;
using System.Collections.Generic;

namespace FlatRent.Models.Responses
{
    public class CreatedFlatResponse
    {
        public Guid Id { get; set; }
        public Dictionary<string, Guid> Images { get; set; }

        public CreatedFlatResponse(Guid id, IEnumerable<KeyValuePair<string, Guid>> images)
        {
            Id = id;
            Images = new Dictionary<string, Guid>(images);
        }
    }
}