﻿using System;
using System.Collections.Generic;
using System.Linq;
using FlatRent.Entities;

namespace FlatRent.Models.Responses
{
    public class CreatedMessageResponse
    {
        public Dictionary<Guid, string> Attachments { get; set; }

        public CreatedMessageResponse(IEnumerable<Attachment> attachments)
        {
            var attachmentArray = attachments.ToArray();
            Attachments = attachmentArray.Any() 
                ? new Dictionary<Guid, string>(
                    attachmentArray.Select(
                        a => new KeyValuePair<Guid, string>(a.Id, a.Name)
                        )
                    ) 
                : null;
        }
    }
}