using AutoMapper;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models.Requests;
using Microsoft.AspNetCore.Http;

namespace FlatRent.Profiles
{
    public class FileMapperProfile : Profile
    {
        public FileMapperProfile()
        {
            CreateMap<IFormFile, File>()
                .IncludeAllDerived()
                .ForMember(i => i.Name, opt => opt.MapFrom(file => file.FileName))
                .ForMember(i => i.MimeType, opt => opt.MapFrom(file => file.ContentType))
                .ForMember(i => i.Bytes, opt => opt.MapFrom(file => file.OpenReadStream().GetByteArray()));

            CreateMap<IFormFile, Image>();
            CreateMap<IFormFile, Attachment>();

            CreateMap<FileMetadata, File>()
                .IncludeAllDerived()
                .ForMember(i => i.Name, opt => opt.MapFrom(file => file.Name))
                .ForMember(i => i.MimeType, opt => opt.MapFrom(_ => ""))
                .ForMember(i => i.Bytes, opt => opt.MapFrom(_ => new byte[0]));
            CreateMap<FileMetadata, Image>();
            CreateMap<FileMetadata, Attachment>();
        }
    }
}