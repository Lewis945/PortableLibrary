using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Models.TvShow
{
    public class ColdFilmTvShowSeasonModel
    {
        public int Index { get; set; }
        public List<ColdFilmTvShowEpisodeModel> Episodes { get; set; }
    }
}