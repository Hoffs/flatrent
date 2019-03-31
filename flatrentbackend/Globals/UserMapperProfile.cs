using AutoMapper;
using FlatRent.Entities;
using FlatRent.Models.Requests;

namespace FlatRent.Globals
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<RegistrationForm, User>();
        }
    }
}