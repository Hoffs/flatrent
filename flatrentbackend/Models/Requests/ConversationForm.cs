using System;
using System.ComponentModel.DataAnnotations;

namespace FlatRent.Models.Requests
{
    public class ConversationForm
    {
        [Required]
        [MaxLength(128)]
        public string Subject { get; set; }
        [Required]
        public Guid RecipientId { get; set; }
    }
}