﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlatRent.Entities
{
    public class Conversation : AuthoredBaseEntity
    {
        public string Subject { get; set; }

        [ForeignKey("Recipient")]
        public Guid RecipientId { get; set; }
        public virtual User Recipient { get; set; }

        [InverseProperty("Conversation")]
        public virtual ICollection<Message> Messages { get; set; }

        [InverseProperty("Conversation")]
        public virtual Agreement Agreement { get; set; }

        [InverseProperty("Conversation")]
        public virtual Incident Incident { get; set; }

        public bool IsAuthorOrRecipient(Guid id)
        {
            return AuthorId == id || RecipientId == id;
        }
    }
}