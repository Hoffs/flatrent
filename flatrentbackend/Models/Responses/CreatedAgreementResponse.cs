using System;
using System.Collections.Generic;

namespace FlatRent.Models.Responses
{
    public class CreatedAgreementResponse
    {
        public Guid Id { get; set; }
        public Dictionary<string, Guid> Attachments { get; set; }

        public CreatedAgreementResponse(Guid id, IEnumerable<KeyValuePair<string, Guid>> files)
        {
            Id = id;
            Attachments = new Dictionary<string, Guid>(files);
        }
    }
}