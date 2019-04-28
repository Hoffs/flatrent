using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using FlatRent.Entities;
using FlatRent.Models.Dtos;
using FlatRent.Models.Requests;

namespace FlatRent.Profiles
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<RegistrationForm, User>();
            CreateMap<User, ShortUserInfo>()
                .IncludeAllDerived();
            CreateMap<User, ShortAgreementUserInfo>();

            CreateMap<User, UserProfile>()
                .IncludeAllDerived()
//                .ForMember(x => x.Flats, opt => opt.Ignore())
//                .ForMember(x => x.Flats, opt => opt.MapFrom(o => o.Flats.Where(f => !f.Deleted).OrderByDescending(f => f.CreatedDate)))
                .ForMember(x => x.TenantAgreementCount,
                    opt => opt.MapFrom(
                        o => o.TenantAgreements.Where(
                            a => a.StatusId == AgreementStatus.Statuses.Accepted ||
                                 a.StatusId == AgreementStatus.Statuses.Ended)))
                .ForMember(x => x.OwnerAgreementCount,
                    opt => opt.MapFrom(
                        o => o.OwnerAgreements.Where(
                            a => a.StatusId == AgreementStatus.Statuses.Accepted ||
                                 a.StatusId == AgreementStatus.Statuses.Ended)));

//            CreateMap<User, UserAgreements>();
//            CreateMap<User, PersonalUserProfile>();
//                .ForMember(x => x.OwnerAgreements, opt => opt.MapFrom(o => o.OwnerAgreements.Where(f => !f.Deleted)))
//                .ForMember(x => x.TenantAgreements, opt => opt.MapFrom(o => o.TenantAgreements.Where(f => !f.Deleted)));
        }
    }
}