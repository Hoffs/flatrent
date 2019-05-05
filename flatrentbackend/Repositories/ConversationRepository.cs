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
using Microsoft.EntityFrameworkCore;
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
                .Where(c => c.Fault == null && c.Agreement == null)
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
            var recipient = await Context.Users.FindAsync(conversation.RecipientId);
            if (recipient == null || recipient.Deleted)
            {
                return (new [] { new FormError(Errors.RecipientDoesNotExist) }, default(Guid));
            }

            var existingConversation = await Context.Conversations.FirstOrDefaultAsync(c =>
                (c.AuthorId == userId || c.AuthorId == conversation.RecipientId) &&
                (c.RecipientId == userId ||
                 c.RecipientId ==
                 conversation.RecipientId) && c.Fault == null && c.Agreement == null);

            if (existingConversation != null)
            {
                return (null, existingConversation.Id);
            }

            var mapped = Mapper.Map<Conversation>(conversation);
            var author = await Context.Users.FindAsync(userId);
            mapped.Subject = $"{recipient.FirstName} {recipient.LastName}, {author.FirstName} {author.LastName}";
            var errors = await base.AddAsync(mapped, userId);
            return (errors, mapped.Id);
        }

        public async Task<(IEnumerable<FormError>, Message)> AddMessage(MessageForm message, Guid conversationId, Guid userId)
        {
            // 2 checks for message sending is expensive
            var agreement = await Context.Agreements.FirstOrDefaultAsync(a => a.ConversationId == conversationId);
            if (agreement != null && (!agreement.Deleted && agreement.StatusId != AgreementStatus.Statuses.Rejected))
            {
                return (new[] { new FormError(Errors.MessageAgreementDeletedOrRejected) }, null);
            }

            var fault = await Context.Faults.FirstOrDefaultAsync(a => a.ConversationId == conversationId);
            if (fault != null && (fault.Deleted || fault.Repaired))
            {
                return (new[] { new FormError(Errors.MessageFaultDeletedOrRepaired) }, null);
            }

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