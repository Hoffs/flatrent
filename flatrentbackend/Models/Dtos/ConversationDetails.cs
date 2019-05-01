using System;

namespace FlatRent.Models.Dtos
{
    public class ConversationDetails
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public ShortUserInfo Recipient { get; set; }
        public ShortUserInfo Author { get; set; }
    }
}