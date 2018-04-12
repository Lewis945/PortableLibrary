using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Models;
using PortableLibrary.Core.Infrastructure.External.Models.TvShow;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// https://online.anidub.com/
    /// </summary>
    public class OnlineAnidubExternalProvider 
    {
        public string ServiceUri => "https://online.anidub.com/";
        public string ServiceName => "AnidubOnline";

        public Task<OnlineAnidubTvShowModel> Extract(string uri)
        {
            throw new NotImplementedException();
        }
    }
}
