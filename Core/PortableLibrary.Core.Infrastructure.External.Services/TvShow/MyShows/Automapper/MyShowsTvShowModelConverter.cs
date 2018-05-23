using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Automapper
{
    public class MyShowsTvShowModelConverter : ITypeConverter<TvShowResponseWrapper, MyShowsTvShowModel>
    {
        public MyShowsTvShowModel Convert(TvShowResponseWrapper source, MyShowsTvShowModel destination,
            ResolutionContext context)
        {
            var model = context.Mapper.Map<MyShowsTvShowModel>(source.Result);

            foreach (var season in model.Seasons)
            {
                season.Episodes = context.Mapper.Map<List<MyShowsTvShowEpisodeModel>>(
                    source.Result.Episodes.Where(e => e.SeasonNumber == season.Index && e.EpisodeNumber > 0));

                season.Specials = context.Mapper.Map<List<MyShowsTvShowEpisodeModel>>(
                    source.Result.Episodes.Where(e => e.SeasonNumber == season.Index && e.EpisodeNumber == 0));
            }

            return model;
        }
    }
}