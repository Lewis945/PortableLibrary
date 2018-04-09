using System;
using System.Threading.Tasks;
using PortableLibrary.Core.External.Services;
using PortableLibrary.Core.Infrastructure.External.Models;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// http://vk.com/
    /// </summary>
    public class VkontakteVideoAlbumExternalProvider : IExternalServiceProvider<VkontakteVideoAlbumTvShowModel>
    {
        public string ServiceUri => "http://vk.com/";
        public string ServiceName => "VkontakteAlbum";

        public Task<VkontakteVideoAlbumTvShowModel> Extract(string uri)
        {
            throw new NotImplementedException();
        }
    }
}