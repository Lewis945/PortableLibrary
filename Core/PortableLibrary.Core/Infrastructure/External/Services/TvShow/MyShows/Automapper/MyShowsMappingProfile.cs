using System.Linq;
using AutoMapper;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Automapper
{
    public class MyShowsMappingProfile : Profile
    {
        public MyShowsMappingProfile()
        {
            CreateMap<EpisodeResponse, MyShowsTvShowSeasonModel>()
                .ForMember(m => m.SeasonIndex, c => c.MapFrom(e => e.SeasonNumber));

            CreateMap<EpisodeResponse, MyShowsTvShowEpisodeModel>();

            CreateMap<ResultResponse, MyShowsTvShowModel>()
                .ForMember(dest => dest.Seasons,
                    opt => opt.MapFrom(src =>
                        src.Episodes.DistinctBy(e => e.SeasonNumber).OrderBy(e => e.SeasonNumber)));

            CreateMap<TvShowResponse, MyShowsTvShowModel>()
                .ConvertUsing<MyShowsTvShowModelConverter>();

            CreateMap<TvShowSearchResponse, TvShowResponse>()
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.Result.FirstOrDefault()));
        }
    }
}