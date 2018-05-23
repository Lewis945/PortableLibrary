using System;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.NewStudio
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