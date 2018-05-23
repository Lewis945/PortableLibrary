using System;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using PortableLibrary.Core.Automapper;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Automapper
{
    public class MyShowsMappingProfile : Profile
    {
        public MyShowsMappingProfile()
        {
            CreateMap<string, DateTimeOffset?>().ConvertUsing(new DateTimeOffsetTypeConverter());

            CreateMap<EpisodeResponse, MyShowsTvShowSeasonModel>()
                .ForMember(dest => dest.Index, opt => opt.MapFrom(src => src.SeasonNumber));

            CreateMap<EpisodeResponse, MyShowsTvShowEpisodeModel>()
                .ForMember(dest => dest.AirDate, opt => opt.MapFrom(src => src.AirDateUTC));

            CreateMap<TvShowResponse, MyShowsTvShowModel>()
                .ForMember(dest => dest.Description,
                    opt => opt.ResolveUsing((src, dest, destMember, res) =>
                    {
                        var textDescription = src.Description != null
                            ? Regex.Replace(src.Description, "<[^>]*>", string.Empty)
                            : string.Empty;
                        return textDescription.ClearString();
                    }))
                .ForMember(dest => dest.Status,
                    opt => opt.ResolveUsing((src, dest, destMember, res) =>
                    {
                        switch (src.Status)
                        {
                            case "Canceled/Ended":
                                return TvShowStatus.CanceledOrEnded;
                            case "Returning Series":
                                return TvShowStatus.Ongoing;
                            case "New Series":
                                return TvShowStatus.NewSeries;
                            case "In Development":
                                return TvShowStatus.InDevelopment;
                            default:
                                throw new ArgumentException($"Status {src.Status} is undefined!", nameof(src.Status));
                        }
                    }))
//                .ForMember(dest => dest.Started,
//                    opt => opt.MapFrom(src => src.Started))
                .ForMember(dest => dest.Seasons,
                    opt => opt.MapFrom(src =>
                        src.Episodes.DistinctBy(e => e.SeasonNumber).OrderBy(e => e.SeasonNumber)));

            CreateMap<TvShowResponseWrapper, MyShowsTvShowModel>()
                .ConvertUsing<MyShowsTvShowModelConverter>();
        }
    }
}