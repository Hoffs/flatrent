namespace FlatRent.Models.Dtos
{
    public class ConversationDetails
    {
        public string Subject { get; set; }
        public ShortUserInfo Recipient { get; set; }
        public ShortUserInfo Author { get; set; }
    }
}