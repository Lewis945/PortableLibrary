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
        public string ProviderUri => "http://vk.com/";

        public Task<VkontakteVideoAlbumTvShowModel> Extract()
        {
            throw new NotImplementedException();
        }
    }
}