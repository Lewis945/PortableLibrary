using AutoMapper;
using PortableLibrary.Core.Database.Entities.BooksLibrary;
using PortableLibrary.Core.SimpleServices.Models;

namespace PortableLibrary.Core.Automapper
{
    public class LibraryProfile : Profile
    {
        public LibraryProfile()
        {
            CreateMap<BooksLibrary, LibraryListModel>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Books.Count));
//                .ForMember(dest => dest.Index, opt => opt.MapFrom(src => src.EpisodeNumber))
//                .ForMember(dest => dest.OriginalTitle, opt => opt.MapFrom(src => src.Title))
//                .ForMember(dest => dest.ImageUri, opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Image) ? src.Image : null))
//                .ForMember(dest => dest.Titles, opt => opt.MapFrom(src => new List<string> {src.Title}));
        }
    }
}