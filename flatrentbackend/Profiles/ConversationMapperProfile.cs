using AutoMapper;
using FlatRent.Entities;
using FlatRent.Models.Requests;

namespace FlatRent.Profiles
{
    public class ConversationMapperProfile : Profile
    {
        public ConversationMapperProfile()
        {
            CreateMap<MessageForm, Message>();
            CreateMap<ConversationForm, Conversation>();
        }
    }
}