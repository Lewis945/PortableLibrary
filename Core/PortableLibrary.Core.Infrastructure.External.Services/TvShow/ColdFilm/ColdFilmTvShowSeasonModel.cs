using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.ColdFilm
{
    public class ColdFilmTvShowSeasonModel
    {
        public int Index { get; set; }
        public List<ColdFilmTvShowEpisodeModel> Episodes { get; set; }
    }
}