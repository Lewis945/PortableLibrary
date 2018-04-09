using PortableLibrary.Core.External.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// http://online.animedia.tv
    /// </summary>
    public class OnlineAnimediaExternalProvider : IExternalServiceProvider<OnlineAnimediaTvShowModel>
    {
        public string ServiceUri => "http://online.animedia.tv";
        public string ServiceName => "AnimediaOnline";

        public Task<OnlineAnimediaTvShowModel> Extract(string uri)
        {
            throw new NotImplementedException();
        }
    }
}
