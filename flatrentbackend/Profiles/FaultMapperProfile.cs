using AutoMapper;
using FlatRent.Entities;
using FlatRent.Models.Dtos;
using FlatRent.Models.Requests;

namespace FlatRent.Profiles
{
    public class FaultMapperProfile : Profile
    {
        public FaultMapperProfile()
        {
            CreateMap<Fault, ShortFaultDetails>();

            CreateMap<FaultForm, Fault>();

            CreateMap<Fault, FaultDetails>()
                .ForMember(f => f.Tenant, opt => opt.MapFrom(o => o.Author))
                .ForMember(f => f.Owner, opt => opt.MapFrom(o => o.Agreement.Flat.Author))
                .ForMember(f => f.Flat, opt => opt.MapFrom(o => o.Agreement.Flat));
        }
    }
}