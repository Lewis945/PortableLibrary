namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response
{
    public class EpisodeResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ShowId { get; set; }
        public int? SeasonNumber { get; set; }
        public int? EpisodeNumber { get; set; }
        public string AirDate { get; set; }
        public string AirDateUTC { get; set; }
        public string Image { get; set; }
        public string ShortName { get; set; }
    }
}