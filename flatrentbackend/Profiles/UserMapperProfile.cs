using AutoMapper;
using FlatRent.Entities;
using FlatRent.Models.Dtos;
using FlatRent.Models.Requests;

namespace FlatRent.Profiles
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<RegistrationForm, User>();
            CreateMap<User, ShortUserInfo>()
//                .ForMember(u => u.Avatar, opt => opt.MapFrom(u => u.AvatarId))
                ;
        }
    }
}