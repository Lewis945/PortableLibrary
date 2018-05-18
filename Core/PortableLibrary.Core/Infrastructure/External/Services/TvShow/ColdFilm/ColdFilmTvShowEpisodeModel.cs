using System;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.ColdFilm
{
    public class ColdFilmTvShowEpisodeModel
    {
        public string Title { get; set; }
        public string ImageUri { get; set; }
        public int Index { get; set; }
        public DateTime DateReleased { get; set; }
    }
}