using System;
using System.Threading.Tasks;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.OnlineAnimedia
{
    /// <summary>
    /// http://online.animedia.tv
    /// </summary>
    public class OnlineAnimediaExternalProvider 
    {
        public string ServiceUri => "http://online.animedia.tv";
        public string ServiceName => "AnimediaOnline";

        public Task<OnlineAnimediaTvShowModel> Extract(string uri)
        {
            throw new NotImplementedException();
        }
    }
}
