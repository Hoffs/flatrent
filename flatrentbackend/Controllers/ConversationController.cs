using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FlatRent.Controllers.Abstractions;
using FlatRent.Controllers.Filters;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models.Dtos;
using FlatRent.Models.Requests;
using FlatRent.Models.Responses;
using FlatRent.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FlatRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : AuthoredBaseEntityController<Conversation>
    {
        private readonly IConversationRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ConversationController(IConversationRepository repository, IMapper mapper, ILogger logger) : base(repository)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet]
        [Authorize]
        public IActionResult GetConversationsAsync([FromQuery] int offset)
        {
            var userConversations = _repository.GetUserConversations(User.GetUserId(), offset);
            var mapped = _mapper.Map<IEnumerable<ConversationDetails>>(userConversations);
            return Ok(mapped);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateConversationsAsync([FromBody] ConversationForm form)
        {
            var (errors, conversationId) = await _repository.AddConversation(form, User.GetUserId());
            return OkOrBadRequest(errors, Ok(new CreatedConversationResponse{ Id = conversationId }));
        }

        [HttpGet("{id}")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> GetConversationAsync([FromRoute] Guid id)
        {
            var conversation = await _repository.GetAsync(id);
            if (!conversation.IsAuthorOrRecipient(User.GetUserId())) return Forbid();
            var mapped = _mapper.Map<ConversationDetails>(conversation);
            return Ok(mapped);
        }

        [HttpPost("{id}")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> CreateMessageAsync([FromRoute] Guid id, [FromBody] MessageForm form)
        {
            var conversation = await _repository.GetAsync(id);
            var userId = User.GetUserId();
            if (!conversation.IsAuthorOrRecipient(userId)) return Forbid();

            var (errors, message) = await _repository.AddMessage(form, id, userId);
            return OkOrBadRequest(errors, Ok(new CreatedMessageResponse(message.Id, message.Attachments)));
        }

        [HttpGet("{id}/messages")]
        [Authorize]
        [EntityMustExist]
        public async Task<IActionResult> GetConversationMessagesAsync([FromRoute] Guid id, [FromQuery] int offset)
        {
            var conversation = await _repository.GetAsync(id);
            if (!conversation.IsAuthorOrRecipient(User.GetUserId())) return Forbid();

            var messages = _repository.GetConversationMessages(id, offset);
            var mapped = _mapper.Map<IEnumerable<MessageDetails>>(messages);
            return Ok(mapped);
        }
    }
}