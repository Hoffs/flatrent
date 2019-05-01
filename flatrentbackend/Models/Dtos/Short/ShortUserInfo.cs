using System;

namespace FlatRent.Models.Dtos
{
    public class ShortUserInfo
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid AvatarId { get; set; }
    }
}