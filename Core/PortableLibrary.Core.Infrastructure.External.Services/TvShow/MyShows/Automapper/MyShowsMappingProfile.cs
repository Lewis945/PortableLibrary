using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using PortableLibrary.Core.Automapper;
using PortableLibrary.Core.Extensions;
using PortableLibrary.Core.External.Services.TvShow.Models;
using PortableLibrary.Core.External.Services.TvShow.Models.DataExtraction;
using PortableLibrary.Core.External.Services.TvShow.Models.Search;
using PortableLibrary.Core.External.Services.TvShow.Models.Tracking;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Automapper
{
    public class MyShowsMappingProfile : Profile
    {
        public MyShowsMappingProfile()
        {
            CreateMap<string, DateTimeOffset?>().ConvertUsing(new DateTimeOffsetTypeConverter());

            #region Data Extraction

            CreateMap<EpisodeResponse, TvShowSeasonDataExtractionModel>()
                .ForMember(dest => dest.Index, opt => opt.MapFrom(src => src.SeasonNumber));

            CreateMap<EpisodeResponse, TvShowEpisodeDataExtractionModel>()
                .ForMember(dest => dest.DateReleasedOrigianl, opt => opt.MapFrom(src => src.AirDateUTC))
                .ForMember(dest => dest.Index, opt => opt.MapFrom(src => src.EpisodeNumber))
                .ForMember(dest => dest.OriginalTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.ImageUri, opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Image) ? src.Image : null))
                .ForMember(dest => dest.Titles, opt => opt.MapFrom(src => new List<string> {src.Title}));

            CreateMap<TvShowResponse, TvShowDataExtractionModel>()
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
                        src.Episodes.DistinctBy(e => e.SeasonNumber).OrderBy(e => e.SeasonNumber)))
                .ForMember(dest => dest.ImageUri,
                    opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Image) ? src.Image : null))
                .ForMember(dest => dest.Titles, opt => opt.MapFrom(src => new List<string> {src.Title}));

            CreateMap<TvShowResponseWrapper, TvShowDataExtractionModel>()
                .ConvertUsing<TvShowEpisodeDataExtractionModelConverter>();

            #endregion

            #region Tracking

            CreateMap<EpisodeResponse, TvShowSeasonTrackingModel>()
                .ForMember(dest => dest.Index, opt => opt.MapFrom(src => src.SeasonNumber));

            CreateMap<EpisodeResponse, TvShowEpisodeTrackingModel>()
                .ForMember(dest => dest.AirDate, opt => opt.MapFrom(src => src.AirDateUTC));

            CreateMap<TvShowResponse, TvShowTrackingModel>()
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
                .ForMember(dest => dest.Seasons,
                    opt => opt.MapFrom(src =>
                        src.Episodes.DistinctBy(e => e.SeasonNumber).OrderBy(e => e.SeasonNumber)));

            CreateMap<TvShowResponseWrapper, TvShowTrackingModel>()
                .ConvertUsing<TvShowEpisodeTrackingModelConverter>();

            #endregion

            #region Search

            CreateMap<TvShowResponse, TvShowSearchModel>()
                .ForMember(dest => dest.Titles, opt => opt.MapFrom(src => new List<string> {src.Title}));

            #endregion
        }
    }
}