using System;
using System.Threading.Tasks;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.MyShows
{
    /// <summary>
    /// https://en.myshows.me/
    /// </summary>
    public class MyShowsExternalProvider
    {
        public string ServiceUri => "https://en.myshows.me/";
        public string ServiceName => "MyShowsMe";

        public Task<MyShowsTvShowModel> Extract(string uri)
        {
            throw new NotImplementedException();
        }
    }
}
