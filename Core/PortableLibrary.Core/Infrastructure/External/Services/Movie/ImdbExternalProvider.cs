using System;
using System.Threading.Tasks;
using PortableLibrary.Core.External.Services;
using PortableLibrary.Core.Infrastructure.External.Models;
using PortableLibrary.Core.Infrastructure.External.Models.Movie;

namespace PortableLibrary.Core.Infrastructure.External.Services.Movie
{
    /// <summary>
    /// http://www.imdb.com
    /// </summary>
    public class ImdbExternalProvider : IExternalServiceProvider<ImdbMovieModel>
    {
        public string ServiceUri => "http://www.imdb.com";
        public string ServiceName => "Imdb";

        public Task<ImdbMovieModel> Extract(string uri)
        {
            throw new NotImplementedException();
        }
    }
}