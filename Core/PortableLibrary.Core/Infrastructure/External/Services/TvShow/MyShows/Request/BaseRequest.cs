namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Request
{
    public abstract class BaseRequest
    {
        public string Jsonrpc { get; set; } = "2.0";
        public int Id { get; set; } = 1;
    }
}