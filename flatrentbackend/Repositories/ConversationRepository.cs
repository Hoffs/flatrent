using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;
using FlatRent.Models.Requests;
using FlatRent.Repositories.Abstractions;
using FlatRent.Repositories.Interfaces;
using Serilog;

namespace FlatRent.Repositories
{
    public class ConversationRepository : AuthoredBaseRepository<Conversation>, IConversationRepository
    {
        private readonly MessageRepository _messageRepository;

        public ConversationRepository(DataContext context, IMapper mapper, ILogger logger) : base(context, mapper, logger)
        {
            _messageRepository = new MessageRepository(context, mapper, logger);
        }

        public IEnumerable<Conversation> GetUserConversations(Guid userId, int offset = 0)
        {
            return Context.Conversations
                .Where(c => c.RecipientId == userId || c.AuthorId == userId)
                .OrderByDescending(c => c.CreatedDate)
                .Paginate(offset);
        }

        public IEnumerable<Message> GetConversationMessages(Guid conversationId, int offset = 0)
        {
            return Context.Messages
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.CreatedDate)
                .Paginate(offset, PaginationConstants.ExtendedPageSize);
        }

        public async Task<(IEnumerable<FormError>, Guid)> AddConversation(ConversationForm conversation, Guid userId)
        {
            var mapped = Mapper.Map<Conversation>(conversation);
            var errors = await base.AddAsync(mapped, userId);
            return (errors, mapped.Id);
        }

        public async Task<(IEnumerable<FormError>, Message)> AddMessage(MessageForm message, Guid conversationId, Guid userId)
        {
            var mapped = Mapper.Map<Message>(message);
            mapped.ConversationId = conversationId;
            mapped.Attachments.SetProperty(a => a.AuthorId, userId);
            var errors = await _messageRepository.AddAsync(mapped, userId);
            return (errors, mapped);
        }

        public async Task<IEnumerable<Message>> GetNewConversationMessages(Guid conversationId, Guid lastMessageId)
        {
            var lastMessage = await _messageRepository.GetAsync(lastMessageId);
            var lastMessageTime = lastMessage?.CreatedDate ?? DateTime.MaxValue;
            return Context.Messages
                .Where(m => m.ConversationId == conversationId)
                .Where(m => m.CreatedDate > lastMessageTime)
                .OrderByDescending(m => m.CreatedDate)
                .Take(200); // Limit to 200 new messages
        }

        private class MessageRepository : AuthoredBaseRepository<Message>
        {
            public MessageRepository(DataContext context, IMapper mapper, ILogger logger) : base(context, mapper, logger)
            {
            }

            public new Task<IEnumerable<FormError>> AddAsync(Message message, Guid userId)
            {
                return base.AddAsync(message, userId);
            }
        }
    }
}