using System;
using System.Threading.Tasks;

namespace PortableLibrary.Core.Infrastructure.External.Services.Movie
{
    /// <summary>
    /// http://www.imdb.com
    /// </summary>
    public class ImdbExternalProvider 
    {
        public string ServiceUri => "http://www.imdb.com";
        public string ServiceName => "Imdb";

        public Task<ImdbMovieModel> Extract(string uri)
        {
            throw new NotImplementedException();
        }
    }
}