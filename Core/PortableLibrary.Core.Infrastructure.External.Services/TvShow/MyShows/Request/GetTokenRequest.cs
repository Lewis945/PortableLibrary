namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Request
{
    class GetTokenRequest
    {
        public string GrantType { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
