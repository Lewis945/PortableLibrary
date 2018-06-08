using AutoMapper;
using PortableLibrary.Core.Database.Entities.BooksLibrary;
using PortableLibrary.Core.Database.Entities.TvShowsLibrary;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.SimpleServices.Models;

namespace PortableLibrary.Core.Automapper
{
    public class LibraryProfile : Profile
    {
        public LibraryProfile()
        {
            CreateMap<BooksLibrary, LibraryListModel>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.GetLibraryType()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Books.Count))
                .ForMember(dest => dest.Public, opt => opt.MapFrom(src => src.IsPublic));
            
            CreateMap<TvShowsLibrary, LibraryListModel>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.GetLibraryType()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.TvShows.Count))
                .ForMember(dest => dest.Public, opt => opt.MapFrom(src => src.IsPublic));
        }
    }
}