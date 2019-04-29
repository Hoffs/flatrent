using System;

namespace FlatRent.Models.Dtos
{
    public class FileInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
    }
}