namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows.Response
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string ExpiresIn { get; set; }
        public string TokenType { get; set; }
        public string Scope { get; set; }
        public string RefreshToken { get; set; }
    }
}
