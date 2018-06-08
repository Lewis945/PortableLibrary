using System.Linq;
using AutoMapper;
using PortableLibrary.Core.Database.Entities.BooksLibrary;
using PortableLibrary.Core.Database.Entities.TvShowsLibrary;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.SimpleServices.Models;

namespace PortableLibrary.Core.Automapper
{
    public class ExtendedLibraryProfile : Profile
    {
        public ExtendedLibraryProfile()
        {
            CreateMap<BooksLibrary, LibraryListModel>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.GetLibraryType()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Books.Count))
                .ForMember(dest => dest.Public, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(dest => dest.Published, opt => opt.MapFrom(src => src.Books.Count(b => b.IsPublished)))
                .ForMember(dest => dest.Favourits, opt => opt.MapFrom(src => src.Books.Count(b => b.IsFavourite)))
                .ForMember(dest => dest.Processing, opt => opt.MapFrom(src => src.Books.Count(b => b.IsProcessing)))
                .ForMember(dest => dest.Processed, opt => opt.MapFrom(src => src.Books.Count(b => b.IsProcessed)))
                .ForMember(dest => dest.Planned, opt => opt.MapFrom(src => src.Books.Count(b => b.IsProcessingPlanned)))
                .ForMember(dest => dest.AreWaitingToBecomeGlobal,
                    opt => opt.MapFrom(src => src.Books.Count(b => b.IsWaitingToBecomeGlobal)));

            CreateMap<TvShowsLibrary, LibraryListModel>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.GetLibraryType()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.TvShows.Count))
                .ForMember(dest => dest.Public, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(dest => dest.Published, opt => opt.MapFrom(src => src.TvShows.Count(b => b.IsPublished)))
                .ForMember(dest => dest.Favourits, opt => opt.MapFrom(src => src.TvShows.Count(b => b.IsFavourite)))
                .ForMember(dest => dest.Processing, opt => opt.MapFrom(src => src.TvShows.Count(b => b.IsProcessing)))
                .ForMember(dest => dest.Processed, opt => opt.MapFrom(src => src.TvShows.Count(b => b.IsProcessed)))
                .ForMember(dest => dest.Planned, opt => opt.MapFrom(src => src.TvShows.Count(b => b.IsProcessingPlanned)))
                .ForMember(dest => dest.AreWaitingToBecomeGlobal,
                    opt => opt.MapFrom(src => src.TvShows.Count(b => b.IsWaitingToBecomeGlobal)));
        }
    }
}