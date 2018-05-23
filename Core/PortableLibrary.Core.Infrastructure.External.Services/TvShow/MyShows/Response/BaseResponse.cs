namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response
{
    public abstract class BaseResponse
    {
        public string Jsonrpc { get; set; }
        public int Id { get; set; }
    }
}