namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Request
{
    public class GetTvShowByIdRequest : BaseRequest
    {
        public string Method { get; set; } = "shows.GetById";
        public RequestParams Params { get; set; }

        public class RequestParams
        {
            public int ShowId { get; set; }
            public bool WithEpisodes { get; set; }
        }
    }
}