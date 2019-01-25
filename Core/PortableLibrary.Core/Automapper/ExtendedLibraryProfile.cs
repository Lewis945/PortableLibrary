using AutoMapper;
using PortableLibrary.Core.Database.Entities.Base;
using PortableLibrary.Core.Database.Entities.BooksLibrary;
using PortableLibrary.Core.Database.Entities.TvShowsLibrary;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.SimpleServices.Models;

namespace PortableLibrary.Core.Automapper
{
    internal static class ExtendedLibraryProfileExtensions
    {
        public static IMappingExpression<TS, TD> AddExtendedLibraryFieldsMappings<TS, TD>(this IMappingExpression<TS, TD> mapping)
            where TS : BaseLibrary
            where TD : LibraryExtendedModel
        {
            return mapping
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.GetLibraryType()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.GetItemsCount(null)))
                .ForMember(dest => dest.Public, opt => opt.MapFrom(src => src.IsPublic))
                .ForMember(dest => dest.Published, opt => opt.MapFrom(src => src.GetItemsCount(b => b.IsPublished)))
                .ForMember(dest => dest.Favourits, opt => opt.MapFrom(src => src.GetItemsCount(b => b.IsFavourite)))
                .ForMember(dest => dest.Processing, opt => opt.MapFrom(src => src.GetItemsCount(b => b.IsProcessing)))
                .ForMember(dest => dest.Processed, opt => opt.MapFrom(src => src.GetItemsCount(b => b.IsProcessed)))
                .ForMember(dest => dest.Planned, opt => opt.MapFrom(src => src.GetItemsCount(b => b.IsProcessingPlanned)))
                .ForMember(dest => dest.AreWaitingToBecomeGlobal,
                    opt => opt.MapFrom(src => src.GetItemsCount(b => b.IsWaitingToBecomeGlobal)));
        }
    }

    public class ExtendedLibraryProfile : Profile
    {
        public ExtendedLibraryProfile()
        {
            CreateMap<BooksLibrary, LibraryListExtendedModel>().AddExtendedLibraryFieldsMappings();
            CreateMap<TvShowsLibrary, LibraryListExtendedModel>().AddExtendedLibraryFieldsMappings();
            CreateMap<BooksLibrary, LibraryExtendedModel>().AddExtendedLibraryFieldsMappings();
            CreateMap<TvShowsLibrary, LibraryExtendedModel>().AddExtendedLibraryFieldsMappings();
        }
    }
}