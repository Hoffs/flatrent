using AutoMapper;
using FlatRent.Entities;
using FlatRent.Models.Dtos;

namespace FlatRent.Profiles
{
    public class InvoiceMapperProfile : Profile
    {
        public InvoiceMapperProfile()
        {
            CreateMap<Invoice, InvoiceDetails>();
        }
    }
}