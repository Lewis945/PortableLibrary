using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PortableLibrary.Core.External.Services.TvShow.Models.Tracking;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Automapper
{
    public class TvShowEpisodeTrackingModelConverter :
        ITypeConverter<TvShowResponseWrapper, TvShowTrackingModel>
    {
        public TvShowTrackingModel Convert(TvShowResponseWrapper source, TvShowTrackingModel destination,
            ResolutionContext context)
        {
            var model = context.Mapper.Map<TvShowTrackingModel>(source.Result);

            foreach (var season in model.Seasons)
            {
                season.Episodes = context.Mapper.Map<List<TvShowEpisodeTrackingModel>>(
                    source.Result.Episodes.Where(e => e.SeasonNumber == season.Index && e.EpisodeNumber > 0));

                season.Specials = context.Mapper.Map<List<TvShowEpisodeTrackingModel>>(
                    source.Result.Episodes.Where(e => e.SeasonNumber == season.Index && e.EpisodeNumber == 0));
            }

            return model;
        }
    }
}