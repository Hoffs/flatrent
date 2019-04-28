using AutoMapper;
using FlatRent.Dtos;
using FlatRent.Entities;

namespace FlatRent.Profiles
{
    public class AgreementsMapperProfile : Profile
    {
        public AgreementsMapperProfile()
        {
            CreateMap<Agreement, ShortAgreementDetails>()
                .ForMember(x => x.FlatName, opt => opt.MapFrom(x => x.Flat.Name))
                .ForMember(x => x.Owner, opt => opt.MapFrom(x => x.Flat.Author));
        }
    }
}