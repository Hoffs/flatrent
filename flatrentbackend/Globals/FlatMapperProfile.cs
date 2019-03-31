using AutoMapper;
using FlatRent.Dtos;
using FlatRent.Entities;
using FlatRent.Models.Requests;

namespace FlatRent.Globals
{
    public class FlatMapperProfile : Profile
    {
        public FlatMapperProfile()
        {
            CreateMap<FlatForm, Flat>();
            CreateMap<FlatForm, Address>();

            CreateMap<Address, FlatListItemAddress>();
            CreateMap<Flat, FlatListItem>();

            CreateMap<RentAgreementForm, Agreement>();
        }
    }
}