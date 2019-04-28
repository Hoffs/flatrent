using AutoMapper;
using FlatRent.Dtos;
using FlatRent.Entities;
using FlatRent.Models.Dtos;
using FlatRent.Models.Requests;
using FlatRent.Models.Requests.Flat;
using FlatRent.Models.Responses;

namespace FlatRent.Profiles
{
    public class FlatMapperProfile : Profile
    {
        public FlatMapperProfile()
        {
            CreateMap<FlatForm, Flat>();
            
            CreateMap<FlatForm, Address>();

            CreateMap<Address, ShortAddress>();

            CreateMap<Flat, ShortFlatDetails>()
                .ForMember(fi => fi.ImageId, opt => opt.MapFrom(f => f.CoverImage.Id));

            CreateMap<RentAgreementForm, Agreement>();

            CreateMap<Flat, FlatDetails>()
                .ForMember(fd => fd.Owner, opt => opt.MapFrom(f => f.Author));
        }
    }
}