using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.ColdFilm
{
    public class ColdFilmTvShowModel
    {
        public string Title { get; set; }
        public List<ColdFilmTvShowSeasonModel> Seasons { get; set; }
    }
}