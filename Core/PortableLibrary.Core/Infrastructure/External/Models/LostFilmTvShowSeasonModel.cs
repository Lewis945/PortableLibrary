using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Models
{
    public class LostFilmTvShowSeasonModel
    {
        public string Title { get; set; }

        public int? TotalEpisodesCount { get; set; }
        public int? Index { get; set; }

        public List<LostFilmTvShowEpisodeModel> Episodes { get; set; }
    }
}