﻿using AutoMapper;
using FlatRent.Entities;
using FlatRent.Models.Dtos;
using FlatRent.Models.Requests.Flat;

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

            CreateMap<Flat, FlatDetails>()
                .ForMember(fd => fd.IsRented, opt => opt.MapFrom(f => f.ActiveAgreement != null))
                .ForMember(fd => fd.Owner, opt => opt.MapFrom(f => f.Author));
        }
    }
}