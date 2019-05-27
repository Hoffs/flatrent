using AutoMapper;
using FlatRent.Entities;
using FlatRent.Models.Dtos;
using FlatRent.Models.Requests;

namespace FlatRent.Profiles
{
    public class AgreementsMapperProfile : Profile
    {
        public AgreementsMapperProfile()
        {
            CreateMap<Agreement, ShortAgreementDetails>()
                .ForMember(x => x.FlatName, opt => opt.MapFrom(x => x.Flat.Name));

            CreateMap<Agreement, AgreementDetails>()
                .ForMember(dto => dto.Owner, opt => opt.MapFrom(o => o.Flat.Author))
                .ForMember(dto => dto.Tenant, opt => opt.MapFrom(o => o.Author));

            CreateMap<AgreementForm, Agreement>()
                .ForMember(a => a.From, opt => opt.MapFrom(o => o.From.Date))
                .ForMember(a => a.To, opt => opt.MapFrom(o => o.To.Date));
        }
    }
}