using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Models
{
    public class LostFilmTvShowSeasonModel
    {
        public int? TotalEpisodesCount { get; set; }
        
        public List<LostFilmTvShowEpisodeModel> Episodes { get; set; }
    }
}