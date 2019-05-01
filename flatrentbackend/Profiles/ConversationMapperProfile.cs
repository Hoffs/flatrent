using AutoMapper;
using FlatRent.Entities;
using FlatRent.Models.Dtos;
using FlatRent.Models.Requests;

namespace FlatRent.Profiles
{
    public class ConversationMapperProfile : Profile
    {
        public ConversationMapperProfile()
        {
            CreateMap<MessageForm, Message>();
            CreateMap<ConversationForm, Conversation>();
            CreateMap<Conversation, ConversationDetails>();
            CreateMap<Message, MessageDetails>();
        }
    }
}