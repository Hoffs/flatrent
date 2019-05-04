﻿using System;
using System.ComponentModel.DataAnnotations;

namespace FlatRent.Models.Requests
{
    public class ConversationForm
    {
        [Required]
        public Guid RecipientId { get; set; }
    }
}