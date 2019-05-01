using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Entities;
using FlatRent.Models;
using FlatRent.Models.Requests;

namespace FlatRent.Repositories.Interfaces
{
    public interface IConversationRepository : IAuthoredBaseRepository<Conversation>
    {
        IEnumerable<Conversation> GetUserConversations(Guid userId, int offset);
        IEnumerable<Message> GetConversationMessages(Guid conversationId, int offset);
        Task<(IEnumerable<FormError>, Guid)> AddConversation(ConversationForm conversation, Guid userId);
        Task<(IEnumerable<FormError>, Message)> AddMessage(MessageForm message, Guid userId);
    }
}