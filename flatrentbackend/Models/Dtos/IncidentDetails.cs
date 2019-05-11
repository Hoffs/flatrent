using System;
using System.Collections.Generic;

namespace FlatRent.Models.Dtos
{
    public class IncidentDetails
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public bool Repaired { get; set; }
        public float Price { get; set; }

        public ShortFlatDetails Flat { get; set; }

        public ShortIncidentUserInfo Tenant { get; set; }
        public ShortIncidentUserInfo Owner { get; set; }

        public ConversationDetails Conversation { get; set; }

        public IEnumerable<FileInfo> Attachments { get; set; }
    }

    public class ShortIncidentUserInfo : ShortUserInfo
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}