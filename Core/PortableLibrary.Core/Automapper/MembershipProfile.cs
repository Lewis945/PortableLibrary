using AutoMapper;
using PortableLibrary.Core.Database.Entities.Membership;
using PortableLibrary.Core.Infrastructure.Models;

namespace PortableLibrary.Core.Automapper
{
    public class MembershipProfile : Profile
    {
        public MembershipProfile()
        {
            CreateMap<RegisterModel, AppUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
