using System;

namespace FlatRent.Models.Dtos
{
    public class ShortConversationDetails
    {
        public string Subject { get; set; }
        public ShortUserInfo Recipient { get; set; }
        public ShortUserInfo Author { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}