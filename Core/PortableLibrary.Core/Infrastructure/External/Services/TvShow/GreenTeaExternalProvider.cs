using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Models;
using PortableLibrary.Core.Infrastructure.External.Models.TvShow;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// http://green-tea.tv/
    /// </summary>
    public class GreenTeaExternalProvider 
    {
        public string ServiceUri => "http://green-tea.tv/";
        public string ServiceName => "GreenTeaTv";

        public Task<GreenTeaTvShowModel> Extract(string uri)
        {
            throw new NotImplementedException();
        }
    }
}
