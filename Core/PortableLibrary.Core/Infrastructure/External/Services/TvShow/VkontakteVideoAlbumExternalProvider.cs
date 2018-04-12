using System;
using System.Threading.Tasks;
using PortableLibrary.Core.Infrastructure.External.Models;
using PortableLibrary.Core.Infrastructure.External.Models.TvShow;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// http://vk.com/
    /// </summary>
    public class VkontakteVideoAlbumExternalProvider 
    {
        public string ServiceUri => "http://vk.com/";
        public string ServiceName => "VkontakteAlbum";

        public Task<VkontakteVideoAlbumTvShowModel> Extract(string uri)
        {
            throw new NotImplementedException();
        }
    }
}