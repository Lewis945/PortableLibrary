using System.Collections.Generic;

namespace PortableLibrary.Core.External.Services.TvShow.Models.Tracking
{
    public class TvShowSeasonTrackingModel
    {
        public int Index { get; set; }
        public List<TvShowEpisodeTrackingModel> Episodes { get; set; }
        public List<TvShowEpisodeTrackingModel> Specials { get; set; }
    }
}