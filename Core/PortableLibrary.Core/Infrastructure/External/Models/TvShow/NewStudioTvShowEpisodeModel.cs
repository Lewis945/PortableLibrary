using System;

namespace PortableLibrary.Core.Infrastructure.External.Models.TvShow
{
    public class NewStudioTvShowEpisodeModel
    {
        public string TvShowTitle { get; set; }
        public string OriginalTvShowSeasonTitle { get; set; }
        public string Quality { get; set; }
        public int? Index { get; set; }
        public DateTime DateReleased { get; set; }
    }
}