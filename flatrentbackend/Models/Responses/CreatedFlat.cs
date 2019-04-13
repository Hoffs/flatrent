using System;
using System.Collections.Generic;

namespace FlatRent.Models.Responses
{
    public class CreatedFlat
    {
        public Guid Id { get; set; }
        public Dictionary<string, Guid> Images { get; set; }

        public CreatedFlat(Guid id, IEnumerable<KeyValuePair<string, Guid>> images)
        {
            Id = id;
            Images = new Dictionary<string, Guid>(images);
        }
    }
}