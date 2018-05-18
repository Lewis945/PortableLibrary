using System;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.LostFilm
{
    public class LostFilmTvShowEpisodeModel
    {
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        
        public DateTime? DateReleased { get; set; }
        public DateTime? DateOriginalReleased { get; set; }
        
        public int? Index { get; set; }
    }
}