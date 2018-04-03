using PortableLibrary.Core.External.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.Movie
{
    /// <summary>
    /// http://www.imdb.com
    /// </summary>
    public class ImdbExternalProvider : IExternalServiceProvider<ImdbMovieModel>
    {
        public string ProviderUri => "http://www.imdb.com";
        
        public Task<ImdbMovieModel> Extract()
        {
            throw new NotImplementedException();
        }
    }
}
