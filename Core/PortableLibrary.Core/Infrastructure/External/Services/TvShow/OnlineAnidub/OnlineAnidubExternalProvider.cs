using System;
using System.Threading.Tasks;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.OnlineAnidub
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
