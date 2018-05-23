namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Request
{
    public class GetTvShowGenresRequest : BaseRequest
    {
        public string Method { get; set; } = "shows.Genres";
    }
}