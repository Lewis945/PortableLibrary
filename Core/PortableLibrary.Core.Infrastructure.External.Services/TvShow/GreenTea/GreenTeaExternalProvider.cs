using System;
using System.Threading.Tasks;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.GreenTea
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
