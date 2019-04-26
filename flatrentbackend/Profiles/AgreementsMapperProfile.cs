﻿using AutoMapper;
using FlatRent.Dtos;
using FlatRent.Entities;

namespace FlatRent.Profiles
{
    public class AgreementsMapperProfile : Profile
    {
        public AgreementsMapperProfile()
        {
            CreateMap<Agreement, RentAgreementListItem>()
                .ForMember(x => x.FlatName, opt => opt.MapFrom(x => x.Flat.Name))
                .ForMember(x => x.FlatAddress, opt => opt.MapFrom(x => x.Flat.Address));
        }
    }
}