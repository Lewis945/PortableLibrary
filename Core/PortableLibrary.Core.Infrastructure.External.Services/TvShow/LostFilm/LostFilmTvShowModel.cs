using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.LostFilm
{
    public class LostFilmTvShowModel
    {
        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string Description { get; set; }

        public string ImageUri { get; set; }
        public byte[] ImageByteArray { get; set; }

        public bool? IsComplete { get; set; }

        public List<string> Genres { get; set; }

        public List<LostFilmTvShowSeasonModel> Seasons { get; set; }
    }
}