using AutoMapper;
using FlatRent.Dtos;
using FlatRent.Entities;

namespace FlatRent.Globals
{
    public class AgreementsMapperProfile : Profile
    {
        public AgreementsMapperProfile()
        {
            CreateMap<RentAgreement, RentAgreementListItem>()
                .ForMember(x => x.FlatName, opt => opt.MapFrom(x => x.Flat.Name))
                .ForMember(x => x.FlatAddress, opt => opt.MapFrom(x => x.Flat.Address));
        }
    }
}