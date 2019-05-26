using System;
using System.Collections.Generic;
using System.Linq;
using FlatRent.Entities;

namespace FlatRent.Models.Responses
{
    public class CreatedAgreementResponse
    {
        public Guid Id { get; set; }
        public Dictionary<Guid, string> Attachments { get; set; }

        public CreatedAgreementResponse(Guid id, IEnumerable<KeyValuePair<Guid, string>> files)
        {
            Id = id;
            if (files != null)
            {
                Attachments = new Dictionary<Guid, string>(files);
            }
        }

        public CreatedAgreementResponse(Guid id, IEnumerable<Attachment> files)
        {
            Id = id;
            if (files != null)
            {
                Attachments = new Dictionary<Guid, string>(files.Select(i => new KeyValuePair<Guid, string>(i.Id, i.Name)));
            }
        }
    }
}