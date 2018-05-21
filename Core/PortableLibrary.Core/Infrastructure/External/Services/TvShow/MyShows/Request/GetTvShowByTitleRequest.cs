namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Request
{
    public class GetTvShowByTitleRequest : BaseRequest
    {
        public string Method { get; set; } = "shows.Search";
        public RequestParams Params { get; set; }

        public class RequestParams
        {
            public string Query { get; set; }
        }
    }
}