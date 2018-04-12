using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PortableLibrary.Core.Infrastructure.External.Models.TvShow;
using PortableLibrary.Core.Utilities;

namespace PortableLibrary.Core.Infrastructure.External.Services.TvShow
{
    /// <summary>
    /// http://newstudio.tv/
    /// </summary>
    public class NewStudioExternalProvider : BaseExternalProvider
    {
        #region Properties

        public override string ServiceUri => "http://newstudio.tv/";
        public override string ServiceName => "NewStudio";

        #endregion

        #region Fields

        private readonly IRetryService _retryService;

        #endregion

        #region .ctor

        public NewStudioExternalProvider(IRetryService retryService)
        {
            _retryService = retryService;
        }

        #endregion

        public async Task<NewStudioTvShowModel> Extract(string uri)
        {
            var model = new NewStudioTvShowModel();

            var web = new HtmlWeb();
            var document = await _retryService.ExecuteAsync(() => web.LoadFromWebAsync(uri));

            var divAccordionInner = document.DocumentNode.SelectNodes(".//div")?
                .FirstOrDefault(n => n.HasClass("accordion-inner"));

            var divsTopic = divAccordionInner?.SelectNodes(".//div")?.Where(n => n.HasClass("topic_id"));

            return model;
        }
    }
}