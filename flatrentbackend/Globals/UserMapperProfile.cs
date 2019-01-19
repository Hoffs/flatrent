using AutoMapper;
using FlatRent.Entities;
using FlatRent.Models.Requests;

namespace FlatRent.Globals
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<RegistrationForm, User>()
                .ForMember(x => x.ClientInformation,
                    opt => opt.MapFrom(v => new ClientInformation {Description = v.Description}));

            CreateMap<RegistrationEmployeeForm, User>()
                .ForMember(x => x.EmployeeInformation,
                    opt => opt.MapFrom(v => new EmployeeInformation {Department = v.Department, Position = v.Position}));
        }
    }
}