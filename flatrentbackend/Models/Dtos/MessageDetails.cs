using System;
using System.Collections.Generic;

namespace FlatRent.Models.Dtos
{
    public class MessageDetails
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public IEnumerable<FileInfo> Attachments { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}