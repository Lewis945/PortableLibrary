using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PortableLibrary.Core.Infrastructure.External.Services.TvShow.OnlineAnidub;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.WorldArt
{
    /// <summary>
    /// http://www.world-art.ru
    /// </summary>
    public class WorldArtExtenalProvider : BaseExternalProvider
    {
        #region Properties

        public override string ServiceUri => "http://www.world-art.ru";
        public override string ServiceName => "WorldArt";

        #endregion

        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public WorldArtExtenalProvider(IRetryService retryService)
        {
            _retryService = retryService;
        }

        #endregion

        #region Public Methods

//        public async Task<OnlineAnidubTvShowModel> Extract(string uri)
//        {
//            var model = new OnlineAnidubTvShowModel();
//
//            var container = await GetSeasonContainerNodeAsync(uri);
//
//            return model;
//        }

        #endregion

        #region Private Methods

       

        #endregion
    }
}