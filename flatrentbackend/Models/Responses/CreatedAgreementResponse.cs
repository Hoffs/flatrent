using System;
using System.Collections.Generic;

namespace FlatRent.Models.Responses
{
    public class CreatedAgreementResponse
    {
        public Guid Id { get; set; }
        public Dictionary<Guid, string> Attachments { get; set; }

        public CreatedAgreementResponse(Guid id, IEnumerable<KeyValuePair<Guid, string>> files)
        {
            Id = id;
            Attachments = new Dictionary<Guid, string>(files);
        }
    }
}