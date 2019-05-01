using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlatRent.Entities
{
    public class Message : AuthoredBaseEntity
    {
        [Required]
        [MaxLength(5000)]
        public string Content { get; set; }

        [InverseProperty("Message")]
        public virtual ICollection<Attachment> Attachments { get; set; }

        [ForeignKey("Conversation")]
        public Guid ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }
    }
}