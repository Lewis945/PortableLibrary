using System;
using System.Threading.Tasks;
using PortableLibrary.Core.External.Services;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// https://www.lostfilm.tv
    /// </summary>
    public class LostFilmExternalProvider : IExternalServiceProvider<LostFilmTvShowModel>
    {
        public string ProviderUri => "https://www.lostfilm.tv";

        public Task<LostFilmTvShowModel> Extract()
        {
            throw new NotImplementedException();
        }
    }
}