using AutoMapper;
using PortableLibrary.Core.Database.Entities.Base;
using PortableLibrary.Core.Database.Entities.BooksLibrary;
using PortableLibrary.Core.Database.Entities.TvShowsLibrary;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.SimpleServices.Models;

namespace PortableLibrary.Core.Automapper
{
    internal static class LibraryProfileExtensions
    {
        public static IMappingExpression<TS, TD> AddLibraryFieldsMappings<TS, TD>(this IMappingExpression<TS, TD> mapping)
            where TS : BaseLibrary
            where TD : LibraryModel
        {
            return mapping
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.GetLibraryType()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.GetItemsCount(null)))
                .ForMember(dest => dest.Public, opt => opt.MapFrom(src => src.IsPublic));
        }
    }

    public class LibraryProfile : Profile
    {
        public LibraryProfile()
        {
            CreateMap<BooksLibrary, LibraryListModel>().AddLibraryFieldsMappings();
            CreateMap<TvShowsLibrary, LibraryListModel>().AddLibraryFieldsMappings();
            CreateMap<BooksLibrary, LibraryModel>().AddLibraryFieldsMappings();
            CreateMap<TvShowsLibrary, LibraryModel>().AddLibraryFieldsMappings();
        }
    }
}