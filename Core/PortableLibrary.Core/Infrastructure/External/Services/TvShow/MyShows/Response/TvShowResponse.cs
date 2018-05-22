using System.Collections.Generic;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response
{
    public class TvShowResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleOriginal { get; set; }
        public string Description { get; set; }
        public int? TotalSeasons { get; set; }
        public string Status { get; set; }
        public string Country { get; set; }
        public string Started { get; set; }
        public string Ended { get; set; }
        public int? Year { get; set; }
        public int? KinopoiskId { get; set; }
        public int? TvrageId { get; set; }
        public int? ImdbId { get; set; }
        public string Image { get; set; }
        public decimal? Rating { get; set; }
        public int? Runtime { get; set; }

        public int[] GenreIds { get; set; }

        public IEnumerable<EpisodeResponse> Episodes { get; set; }
    }
}