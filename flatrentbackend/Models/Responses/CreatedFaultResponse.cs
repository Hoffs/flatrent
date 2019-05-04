using System;
using System.Collections.Generic;
using System.Linq;
using FlatRent.Entities;

namespace FlatRent.Models.Responses
{
    public class CreatedFaultResponse
    {
        public Guid Id { get; set; }
        public Dictionary<Guid, string> Attachments { get; set; }

        public CreatedFaultResponse(Guid id, IEnumerable<Attachment> attachments)
        {
            Id = id;
            var attachmentArray = attachments == null ? new Attachment[0] : attachments.ToArray();
            Attachments = new Dictionary<Guid, string>(
                attachmentArray.Select(
                    a => new KeyValuePair<Guid, string>(a.Id, a.Name)
                )
            );
        }
    }
}