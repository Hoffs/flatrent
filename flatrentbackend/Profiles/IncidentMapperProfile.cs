using AutoMapper;
using FlatRent.Entities;
using FlatRent.Models.Dtos;
using FlatRent.Models.Requests;

namespace FlatRent.Profiles
{
    public class IncidentMapperProfile : Profile
    {
        public IncidentMapperProfile()
        {
            CreateMap<IncidentForm, Incident>();

            CreateMap<Incident, ShortIncidentDetails>();
            CreateMap<Incident, IncidentDetails>()
                .ForMember(f => f.Tenant, opt => opt.MapFrom(o => o.Author))
                .ForMember(f => f.Owner, opt => opt.MapFrom(o => o.Agreement.Flat.Author))
                .ForMember(f => f.Flat, opt => opt.MapFrom(o => o.Agreement.Flat));
        }
    }
}