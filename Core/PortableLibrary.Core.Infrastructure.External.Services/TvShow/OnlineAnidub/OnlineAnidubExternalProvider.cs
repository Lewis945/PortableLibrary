using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow.OnlineAnidub
{
    /// <summary>
    /// https://online.anidub.com/
    /// </summary>
    public class OnlineAnidubExternalProvider : BaseExternalProvider
    {
        #region Properties

        public override string ServiceUri => "https://online.anidub.com/";
        public override string ServiceName => "AnidubOnline";

        #endregion

        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public OnlineAnidubExternalProvider(IRetryService retryService)
        {
            _retryService = retryService;
        }

        #endregion

        #region Public Methods

        public async Task<OnlineAnidubTvShowModel> Extract(string uri)
        {
            var model = new OnlineAnidubTvShowModel();

            var container = await GetSeasonContainerNodeAsync(uri);

            return model;
        }

        #endregion

        #region Private Methods

        private async Task<HtmlNode> GetSeasonContainerNodeAsync(string uri)
        {
            var web = new HtmlWeb();
            var document = await _retryService.ExecuteAsync(() => web.LoadFromWebAsync(uri, Encoding.UTF8));
            var divAllEntries = document.DocumentNode.SelectSingleNode("//div[@id='allEntries']");
            return divAllEntries;
        }

        #endregion
    }
}