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
        public string ProviderUri => "http://coldfilm.tv/";
        public Task<ColdFilmTvShowModel> Extract()
        {
            throw new NotImplementedException();
        }
    }
}
