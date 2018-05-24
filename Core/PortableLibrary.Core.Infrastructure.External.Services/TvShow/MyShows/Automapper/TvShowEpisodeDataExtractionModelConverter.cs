using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PortableLibrary.Core.External.Services.TvShow.Models.DataExtraction;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Automapper
{
    public class TvShowEpisodeDataExtractionModelConverter :
        ITypeConverter<TvShowResponseWrapper, TvShowDataExtractionModel>
    {
        public TvShowDataExtractionModel Convert(TvShowResponseWrapper source, TvShowDataExtractionModel destination,
            ResolutionContext context)
        {
            var model = context.Mapper.Map<TvShowDataExtractionModel>(source.Result);

            foreach (var season in model.Seasons)
            {
                season.Episodes = context.Mapper.Map<List<TvShowEpisodeDataExtractionModel>>(
                    source.Result.Episodes.Where(e => e.SeasonNumber == season.Index && e.EpisodeNumber > 0));

                season.Specials = context.Mapper.Map<List<TvShowEpisodeDataExtractionModel>>(
                    source.Result.Episodes.Where(e => e.SeasonNumber == season.Index && e.EpisodeNumber == 0));
            }

            return model;
        }
    }
}