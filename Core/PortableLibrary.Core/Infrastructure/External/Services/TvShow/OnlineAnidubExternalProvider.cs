using PortableLibrary.Core.External.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// https://online.anidub.com/
    /// </summary>
    public class OnlineAnidubExternalProvider : IExternalServiceProvider<OnlineAnidubTvShowModel>
    {
        public string ServiceUri => "https://online.anidub.com/";
        public string ServiceName => "AnidubOnline";

        public Task<OnlineAnidubTvShowModel> Extract(string uri)
        {
            throw new NotImplementedException();
        }
    }
}
