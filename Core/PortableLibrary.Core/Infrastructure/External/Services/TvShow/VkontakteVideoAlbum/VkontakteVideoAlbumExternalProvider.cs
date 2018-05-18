using System;
using System.Threading.Tasks;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.VkontakteVideoAlbum
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