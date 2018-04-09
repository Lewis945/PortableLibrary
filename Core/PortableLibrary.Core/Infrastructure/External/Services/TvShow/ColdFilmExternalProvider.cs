using PortableLibrary.Core.External.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// http://coldfilm.tv/
    /// </summary>
    public class ColdFilmExternalProvider : IExternalServiceProvider<ColdFilmTvShowModel>
    {
        public string ServiceUri => "http://coldfilm.tv/";
        public string ServiceName => "ColdFilm";

        public Task<ColdFilmTvShowModel> Extract(string uri)
        {
            throw new NotImplementedException();
        }
    }
}
